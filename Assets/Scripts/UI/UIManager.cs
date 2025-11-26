using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Steampunk UI Manager - Real-time adjustable health bar system with shard explosions
/// </summary>
public class SteampunkUIManager : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public PlayerCombatStatsSO playerStats;
    public Canvas canvas;
    public RectTransform healthBarContainer;

    [Header("Frame Settings")]
    public Sprite frameSprite;
    public Vector2 framePosition = new Vector2(100, -100);
    [Range(0.1f, 10f)]
    public float frameScale = 1f;
    public bool anchorFrameToTopLeft = true;

    [Header("Bulb Settings")]
    public Sprite bulbLitSprite;
    public Sprite bulbBrokenSprite;
    public float bulbStartXOffset = 450f;
    public float bulbYOffset = 180f;
    public float bulbSpacing = 80f;
    [Range(0.1f, 10f)]
    public float bulbScale = 1f;

    [Header("Connector Settings")]
    public Sprite connectorSprite;
    public float connectorYOffset = 50f;
    public float connectorXAdjust = 0f;
    [Range(0.1f, 10f)]
    public float connectorScale = 1f;

    [Header("Real-Time Adjustment")]
    public bool liveUpdateEnabled = true;

    [Header("Standalone Testing (No PlayerHealth Required)")]
    public bool standaloneTestMode = false;
    [Range(1, 20)]
    public int standaloneMaxHealth = 5;
    [Range(0, 20)]
    public int standaloneCurrentHealth = 5;

    [Header("Quick Test Buttons")]
    public bool testDamage = false;
    public bool testHeal = false;
    public bool testAddMaxHealth = false;
    public bool testHealToFull = false;

    [Header("Effects")]
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound;
    public AudioClip healSound;

    [Header("Shard Explosion System")]
    public BulbShardExplosion shardExplosion;
    public bool enableShardExplosion = true;

    // Cached values for detecting changes
    private Vector2 _lastFramePosition;
    private float _lastFrameScale;
    private float _lastBulbStartXOffset;
    private float _lastBulbYOffset;
    private float _lastBulbSpacing;
    private float _lastBulbScale;
    private float _lastConnectorYOffset;
    private float _lastConnectorXAdjust;
    private float _lastConnectorScale;
    private int _lastStandaloneMaxHealth;
    private int _lastStandaloneCurrentHealth;

    private List<GameObject> bulbObjects = new List<GameObject>();
    private List<Image> bulbImages = new List<Image>();
    private List<GameObject> connectorObjects = new List<GameObject>();
    private GameObject frameObject;
    private int displayedHealth;
    private int displayedMaxHealth;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        ValidateCanvasSetup();
        CacheCurrentValues();
    }

    private void ValidateCanvasSetup()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        }

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        scaler.referencePixelsPerUnit = 100;
    }

    private void Start()
    {
        if (!standaloneTestMode && playerStats == null)
        {
            Debug.LogError("PlayerCombatStatsSO not assigned! Enable 'Standalone Test Mode' to test without it.");
            return;
        }

        if (bulbLitSprite == null || bulbBrokenSprite == null)
        {
            Debug.LogError("Bulb sprites not assigned!");
            return;
        }

        if (healthBarContainer == null)
        {
            Debug.LogError("Health Bar Container not assigned!");
            return;
        }

        InitializeHealthBar();
    }

    private void Update()
    {
        HandleTestingControls();

        if (liveUpdateEnabled)
        {
            CheckForChanges();
        }

        if (standaloneTestMode)
        {
            HandleStandaloneMode();
        }
        else
        {
            HandleNormalMode();
        }
    }

    private void HandleTestingControls()
    {
        if (standaloneTestMode)
        {
            if (testDamage)
            {
                testDamage = false;
                standaloneCurrentHealth = Mathf.Max(0, standaloneCurrentHealth - 1);
            }

            if (testHeal)
            {
                testHeal = false;
                standaloneCurrentHealth = Mathf.Min(standaloneMaxHealth, standaloneCurrentHealth + 1);
            }

            if (testAddMaxHealth)
            {
                testAddMaxHealth = false;
                standaloneMaxHealth++;
            }

            if (testHealToFull)
            {
                testHealToFull = false;
                standaloneCurrentHealth = standaloneMaxHealth;
            }
        }
        else
        {
            if (testDamage && playerHealth != null)
            {
                testDamage = false;
                playerHealth.Damage(1);
            }

            if (testHeal && playerHealth != null)
            {
                testHeal = false;
                playerHealth.Heal(1);
            }

            if (testAddMaxHealth && playerStats != null)
            {
                testAddMaxHealth = false;
                playerStats.MaxHealth++;
            }

            if (testHealToFull && playerHealth != null)
            {
                testHealToFull = false;
                playerHealth.HealToMax();
            }
        }
    }

    private void HandleStandaloneMode()
    {
        standaloneCurrentHealth = Mathf.Clamp(standaloneCurrentHealth, 0, standaloneMaxHealth);

        // Handle max health changes
        if (standaloneMaxHealth != _lastStandaloneMaxHealth)
        {
            if (standaloneMaxHealth > displayedMaxHealth)
            {
                int increase = standaloneMaxHealth - displayedMaxHealth;
                for (int i = 0; i < increase; i++)
                {
                    CreateBulb(displayedMaxHealth + i);
                }

                RecreateAllConnectors(standaloneMaxHealth);
            }
            else
            {
                InitializeHealthBar();
            }

            displayedMaxHealth = standaloneMaxHealth;
            _lastStandaloneMaxHealth = standaloneMaxHealth;
        }

        // Handle current health changes
        if (standaloneCurrentHealth != _lastStandaloneCurrentHealth)
        {
            if (standaloneCurrentHealth < displayedHealth)
            {
                int damageTaken = displayedHealth - standaloneCurrentHealth;
                for (int i = 0; i < damageTaken; i++)
                {
                    int rightmostLit = FindRightmostLitBulb();
                    if (rightmostLit >= 0)
                    {
                        StartCoroutine(BreakSpecificBulb(rightmostLit));
                    }
                }
            }
            else if (standaloneCurrentHealth > displayedHealth)
            {
                int healthGained = standaloneCurrentHealth - displayedHealth;
                HealBulbs(healthGained);
            }

            displayedHealth = standaloneCurrentHealth;
            _lastStandaloneCurrentHealth = standaloneCurrentHealth;
        }
    }

    private void HandleNormalMode()
    {
        int currentPlayerHealth = playerHealth != null ? playerHealth.CurrentHealth : 0;

        // Handle damage
        if (currentPlayerHealth < displayedHealth)
        {
            int damageTaken = displayedHealth - currentPlayerHealth;
            
            StartCoroutine(BreakNumBulbs(damageTaken));

            displayedHealth = currentPlayerHealth;
        }

        // Handle healing
        if (currentPlayerHealth > displayedHealth)
        {
            int healthGained = currentPlayerHealth - displayedHealth;
            HealBulbs(healthGained);
            displayedHealth = currentPlayerHealth;
        }

        // Handle max health increase
        if (playerStats != null && playerStats.MaxHealth > displayedMaxHealth)
        {
            int increase = playerStats.MaxHealth - displayedMaxHealth;
            for (int i = 0; i < increase; i++)
            {
                CreateBulb(displayedMaxHealth + i);
            }

            RecreateAllConnectors(playerStats.MaxHealth);
            displayedMaxHealth = playerStats.MaxHealth;
            displayedHealth = currentPlayerHealth;
        }

        // Handle max health decrease
        if (playerStats != null && playerStats.MaxHealth < displayedMaxHealth)
        {
            InitializeHealthBar();
        }
    }

    private void CheckForChanges()
    {
        bool needsUpdate = false;

        if (_lastFramePosition != framePosition || _lastFrameScale != frameScale)
        {
            UpdateFrameTransform();
            needsUpdate = true;
        }

        if (_lastBulbStartXOffset != bulbStartXOffset ||
            _lastBulbYOffset != bulbYOffset ||
            _lastBulbSpacing != bulbSpacing ||
            _lastBulbScale != bulbScale ||
            _lastFramePosition != framePosition)
        {
            UpdateAllBulbTransforms();
            needsUpdate = true;
        }

        if (_lastConnectorYOffset != connectorYOffset ||
            _lastConnectorXAdjust != connectorXAdjust ||
            _lastConnectorScale != connectorScale ||
            _lastBulbStartXOffset != bulbStartXOffset ||
            _lastBulbSpacing != bulbSpacing ||
            _lastBulbYOffset != bulbYOffset ||
            _lastFramePosition != framePosition)
        {
            UpdateAllConnectorTransforms();
            needsUpdate = true;
        }

        if (needsUpdate)
        {
            CacheCurrentValues();
        }
    }

    private void CacheCurrentValues()
    {
        _lastFramePosition = framePosition;
        _lastFrameScale = frameScale;
        _lastBulbStartXOffset = bulbStartXOffset;
        _lastBulbYOffset = bulbYOffset;
        _lastBulbSpacing = bulbSpacing;
        _lastBulbScale = bulbScale;
        _lastConnectorYOffset = connectorYOffset;
        _lastConnectorXAdjust = connectorXAdjust;
        _lastConnectorScale = connectorScale;
    }

    private void UpdateFrameTransform()
    {
        if (frameObject == null) return;
        RectTransform rect = frameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = framePosition;
        rect.localScale = Vector3.one * frameScale;
    }

    private void UpdateAllBulbTransforms()
    {
        for (int i = 0; i < bulbObjects.Count; i++)
        {
            if (bulbObjects[i] != null)
            {
                UpdateBulbTransform(bulbObjects[i], i);
            }
        }
    }

    private void UpdateBulbTransform(GameObject bulbObj, int index)
    {
        RectTransform rect = bulbObj.GetComponent<RectTransform>();
        float xPos = framePosition.x + bulbStartXOffset + (index * bulbSpacing);
        float yPos = framePosition.y + (anchorFrameToTopLeft ? -bulbYOffset : bulbYOffset);
        rect.anchoredPosition = new Vector2(xPos, yPos);
        rect.localScale = Vector3.one * bulbScale;
    }

    private void UpdateAllConnectorTransforms()
    {
        for (int i = 0; i < connectorObjects.Count; i++)
        {
            if (connectorObjects[i] != null)
            {
                UpdateConnectorTransform(connectorObjects[i], i + 1);
            }
        }
    }

    private void UpdateConnectorTransform(GameObject connectorObj, int bulbIndex)
    {
        RectTransform rect = connectorObj.GetComponent<RectTransform>();

        float prevBulbX = framePosition.x + bulbStartXOffset + ((bulbIndex - 1) * bulbSpacing);
        float currentBulbX = framePosition.x + bulbStartXOffset + (bulbIndex * bulbSpacing);
        float connectorX = (prevBulbX + currentBulbX) / 2f + connectorXAdjust;
        float connectorY = framePosition.y + (anchorFrameToTopLeft ? -bulbYOffset : bulbYOffset) + connectorYOffset;

        rect.anchoredPosition = new Vector2(connectorX, connectorY);
        rect.localScale = Vector3.one * connectorScale;
    }

    private void InitializeHealthBar()
    {
        print("Bar intializing");
        ClearHealthBar();

        if (frameSprite != null)
        {
            CreateFrame();
        }

        if (standaloneTestMode)
        {
            displayedMaxHealth = standaloneMaxHealth;
            displayedHealth = standaloneCurrentHealth;
            _lastStandaloneMaxHealth = standaloneMaxHealth;
            _lastStandaloneCurrentHealth = standaloneCurrentHealth;
        }
        else
        {
            displayedMaxHealth = playerStats != null ? playerStats.MaxHealth : 3;
            displayedHealth = displayedMaxHealth;
        }
        
        for (int i = 0; i < displayedMaxHealth; i++)
        {
            CreateBulb(i);
        }

        for (int i = 1; i < displayedMaxHealth; i++)
        {
            CreateConnector(i);
        }

        // Set initial bulb states based on current health
        for (int i = 0; i < bulbImages.Count; i++)
        {
            bulbImages[i].sprite = (i < displayedHealth) ? bulbLitSprite : bulbBrokenSprite;
        }

        CacheCurrentValues();
    }

    private void ClearHealthBar()
    {
        foreach (var obj in bulbObjects)
            if (obj != null) Destroy(obj);
        foreach (var obj in connectorObjects)
            if (obj != null) Destroy(obj);

        if (frameObject != null)
        {
            Destroy(frameObject);
            frameObject = null;
        }

        bulbObjects.Clear();
        bulbImages.Clear();
        connectorObjects.Clear();
    }

    private void CreateFrame()
    {
        if (healthBarContainer == null || frameSprite == null) return;

        frameObject = new GameObject("SteampunkFrame");
        frameObject.transform.SetParent(healthBarContainer, false);

        RectTransform rect = frameObject.AddComponent<RectTransform>();

        if (anchorFrameToTopLeft)
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
        }
        else
        {
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }

        rect.sizeDelta = new Vector2(frameSprite.rect.width, frameSprite.rect.height);
        rect.anchoredPosition = framePosition;
        rect.localScale = Vector3.one * frameScale;

        Image img = frameObject.AddComponent<Image>();
        img.sprite = frameSprite;
        img.preserveAspect = false;
        img.raycastTarget = false;

        frameObject.transform.SetAsFirstSibling();
    }

    private void CreateBulb(int index)
    {
        GameObject bulbObj = new GameObject($"HealthBulb_{index}");
        bulbObj.transform.SetParent(healthBarContainer, false);

        RectTransform rect = bulbObj.AddComponent<RectTransform>();

        if (anchorFrameToTopLeft)
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }

        rect.sizeDelta = new Vector2(bulbLitSprite.rect.width, bulbLitSprite.rect.height);
        rect.localScale = Vector3.one * bulbScale;

        float xPos = framePosition.x + bulbStartXOffset + (index * bulbSpacing);
        float yPos = framePosition.y + (anchorFrameToTopLeft ? -bulbYOffset : bulbYOffset);
        rect.anchoredPosition = new Vector2(xPos, yPos);

        Image img = bulbObj.AddComponent<Image>();
        img.sprite = bulbLitSprite;
        img.preserveAspect = false;
        img.raycastTarget = false;

        bulbObjects.Add(bulbObj);
        bulbImages.Add(img);
    }

    private void CreateConnector(int index)
    {
        GameObject connectorObj = new GameObject($"Connector_{index - 1}_to_{index}");
        connectorObj.transform.SetParent(healthBarContainer, false);

        RectTransform rect = connectorObj.AddComponent<RectTransform>();

        if (anchorFrameToTopLeft)
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }

        if (connectorSprite != null)
        {
            rect.sizeDelta = new Vector2(connectorSprite.rect.width, connectorSprite.rect.height);
        }
        else
        {
            rect.sizeDelta = new Vector2(50, 20);
        }

        rect.localScale = Vector3.one * connectorScale;
        UpdateConnectorTransform(connectorObj, index);

        Image img = connectorObj.AddComponent<Image>();
        img.sprite = connectorSprite;
        img.preserveAspect = false;
        img.raycastTarget = false;

        if (connectorSprite == null)
        {
            img.color = new Color(0.6f, 0.4f, 0.2f);
        }

        connectorObjects.Add(connectorObj);
    }

    private void RecreateAllConnectors(int maxHealth)
    {
        foreach (var connector in connectorObjects)
            if (connector != null) Destroy(connector);
        connectorObjects.Clear();

        for (int i = 1; i < maxHealth; i++)
        {
            CreateConnector(i);
        }
    }

    private int FindRightmostLitBulb()
    {
        for (int i = bulbImages.Count - 1; i >= 0; i--)
        {
            if (bulbImages[i].sprite == bulbLitSprite)
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator BreakNumBulbs(int num_to_break)
    {
        int rightmostIndex = FindRightmostLitBulb();
        for(int i = rightmostIndex; i > rightmostIndex - num_to_break && i > 0; i--)
            yield return StartCoroutine(BreakSpecificBulb(i));
    }

    private IEnumerator BreakSpecificBulb(int bulbIndex)
    {
        if (bulbIndex < 0 || bulbIndex >= bulbObjects.Count) yield break;

        GameObject bulbObj = bulbObjects[bulbIndex];
        Image bulbImg = bulbImages[bulbIndex];

        if (bulbImg.sprite != bulbLitSprite) yield break;

        // Spawn shard explosion (includes its own delay)
        if (enableShardExplosion && shardExplosion != null)
        {
            shardExplosion.SpawnShardExplosion(bulbObj.transform.position, healthBarContainer);
        }

        // Start shake and sound/particle effects simultaneously
        StartCoroutine(ShakeBulb(bulbObj, bulbIndex));

        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab,
                bulbObj.transform.position,
                Quaternion.identity,
                transform);
            Destroy(explosion, 1f);
        }

        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        yield return new WaitForSeconds(0.3f);

        bulbImg.sprite = bulbBrokenSprite;
    }

    private void HealBulbs(int amount)
    {
        int healed = 0;
        for (int i = 0; i < bulbImages.Count && healed < amount; i++)
        {
            if (bulbImages[i].sprite == bulbBrokenSprite)
            {
                bulbImages[i].sprite = bulbLitSprite;
                StartCoroutine(GlowBulb(bulbObjects[i]));
                healed++;
            }
        }

        if (healSound != null && audioSource != null && amount > 0)
        {
            audioSource.PlayOneShot(healSound);
        }
    }

    private IEnumerator ShakeBulb(GameObject bulb, int bulbIndex)
    {
        RectTransform rect = bulb.GetComponent<RectTransform>();
        if (rect == null) yield break;

        float intendedX = framePosition.x + bulbStartXOffset + (bulbIndex * bulbSpacing);
        float intendedY = framePosition.y + (anchorFrameToTopLeft ? -bulbYOffset : bulbYOffset);
        Vector2 intendedPos = new Vector2(intendedX, intendedY);

        float duration = 0.3f;
        float elapsed = 0f;
        float intensity = 5f;

        while (elapsed < duration)
        {
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);
            rect.anchoredPosition = intendedPos + new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = intendedPos;
    }

    private IEnumerator GlowBulb(GameObject bulb)
    {
        Image img = bulb.GetComponent<Image>();
        if (img == null) yield break;

        Color originalColor = img.color;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float glow = Mathf.Sin(t * Mathf.PI * 3);
            img.color = Color.Lerp(originalColor, Color.yellow, glow * 0.5f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        img.color = originalColor;
    }

    [ContextMenu("Rebuild Health Bar")]
    private void RebuildHealthBar()
    {
        if (Application.isPlaying)
        {
            InitializeHealthBar();
        }
    }
}