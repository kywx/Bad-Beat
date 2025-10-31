using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Main UI Manager - handles the steampunk health bar display
/// </summary>
public class SteampunkUIManager : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public PlayerCombatStatsSO playerStats;

    [Header("Health Bar Settings")]
    public GameObject healthBulbPrefab;
    public GameObject connectingBarPrefab;
    public Transform bulbContainer;
    public float bulbSpacing = 80f;
    public float startX = 100f;

    [Header("Bulb Size (Optional)")]
    public bool useFixedBulbSize = false;
    public Vector2 fixedBulbSize = new Vector2(40, 60);

    [Header("Connector Bar Adjustments")]
    public Vector2 connectorOffset = Vector2.zero; // X and Y offset for fine-tuning
    public Vector2 connectorSize = Vector2.zero; // Override size (leave zero for prefab size)
    public float connectorYPosition = 20f; // Y position relative to bulb (positive = up from center)

    [Header("Sprites")]
    public Sprite bulbLitSprite;
    public Sprite bulbBrokenSprite;

    [Header("Effects")]
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound;
    public AudioClip healSound;

    private List<HealthBulbModule> healthBulbs = new List<HealthBulbModule>();
    private int displayedHealth;
    private int displayedMaxHealth;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerCombatStatsSO not assigned to SteampunkUIManager!");
            return;
        }

        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        displayedMaxHealth = playerStats.MaxHealth;
        displayedHealth = displayedMaxHealth;

        // Clear existing bulbs
        foreach (var bulb in healthBulbs)
        {
            if (bulb != null && bulb.gameObject != null)
                Destroy(bulb.gameObject);
        }
        healthBulbs.Clear();

        // Create bulbs based on max health
        for (int i = 0; i < displayedMaxHealth; i++)
        {
            AddHealthBulb(i);
        }
    }

    private void Update()
    {
        // Check if player's health has changed
        int currentPlayerHealth = GetPlayerCurrentHealth();

        // Handle damage
        if (currentPlayerHealth < displayedHealth)
        {
            int damageTaken = displayedHealth - currentPlayerHealth;
            for (int i = 0; i < damageTaken; i++)
            {
                StartCoroutine(ExplodeRightmostBulb());
            }
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
        if (playerStats.MaxHealth > displayedMaxHealth)
        {
            int increase = playerStats.MaxHealth - displayedMaxHealth;
            for (int i = 0; i < increase; i++)
            {
                AddHealthBulb(displayedMaxHealth + i);
            }
            displayedMaxHealth = playerStats.MaxHealth;
            displayedHealth = currentPlayerHealth;
        }
    }

    private int GetPlayerCurrentHealth()
    {
        if (playerHealth == null) return 0;
        return playerHealth.CurrentHealth;
    }

    private void AddHealthBulb(int index)
    {
        // Create connecting bar (except for first bulb)
        GameObject bar = null;
        if (index > 0)
        {
            bar = Instantiate(connectingBarPrefab, bulbContainer);
            RectTransform barRect = bar.GetComponent<RectTransform>();

            // Position bar between previous bulb and current bulb
            float prevBulbX = startX + ((index - 1) * bulbSpacing);
            float currentBulbX = startX + (index * bulbSpacing);
            float barX = (prevBulbX + currentBulbX) / 2f; // Centered between bulbs

            // Use connectorYPosition for vertical placement (near top of bulbs)
            barRect.anchoredPosition = new Vector2(barX + connectorOffset.x, connectorYPosition + connectorOffset.y);

            // Apply custom size if specified
            if (connectorSize != Vector2.zero)
            {
                barRect.sizeDelta = connectorSize;
            }
        }

        // Create bulb
        GameObject bulbObj = Instantiate(healthBulbPrefab, bulbContainer);
        RectTransform bulbRect = bulbObj.GetComponent<RectTransform>();

        // Ensure all bulbs are at the same Y position (0)
        bulbRect.anchoredPosition = new Vector2(startX + (index * bulbSpacing), 0);

        // Set anchors and pivot to ensure consistent positioning
        bulbRect.anchorMin = new Vector2(0, 0.5f);
        bulbRect.anchorMax = new Vector2(0, 0.5f);
        bulbRect.pivot = new Vector2(0.5f, 0.5f);

        HealthBulbModule bulbModule = bulbObj.GetComponent<HealthBulbModule>();
        if (bulbModule == null)
            bulbModule = bulbObj.AddComponent<HealthBulbModule>();

        bulbModule.Initialize(index, bulbLitSprite, bulbBrokenSprite, bar);

        // Apply fixed size if enabled
        if (useFixedBulbSize)
        {
            bulbRect.sizeDelta = fixedBulbSize;
        }

        bulbModule.SetLit(true);

        healthBulbs.Add(bulbModule);
    }

    private IEnumerator ExplodeRightmostBulb()
    {
        // Find rightmost lit bulb
        HealthBulbModule rightmostBulb = null;
        for (int i = healthBulbs.Count - 1; i >= 0; i--)
        {
            if (healthBulbs[i].IsLit)
            {
                rightmostBulb = healthBulbs[i];
                break;
            }
        }

        if (rightmostBulb == null) yield break;

        // Play explosion animation
        rightmostBulb.PlayExplosionAnimation();

        // Spawn explosion effect
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab,
                rightmostBulb.transform.position,
                Quaternion.identity,
                transform);
            Destroy(explosion, 1f);
        }

        // Play sound
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Wait for animation
        yield return new WaitForSeconds(0.3f);

        // Set to broken state
        rightmostBulb.SetLit(false);

        // Update bar visibility
        UpdateConnectingBars();
    }

    private void HealBulbs(int amount)
    {
        // Find leftmost broken bulbs and light them up
        int healed = 0;
        for (int i = 0; i < healthBulbs.Count && healed < amount; i++)
        {
            if (!healthBulbs[i].IsLit)
            {
                healthBulbs[i].SetLit(true);
                healthBulbs[i].PlayHealAnimation();
                healed++;
            }
        }

        // Play heal sound
        if (healSound != null && audioSource != null && amount > 0)
        {
            audioSource.PlayOneShot(healSound);
        }

        UpdateConnectingBars();
    }

    private void UpdateConnectingBars()
    {
        // Find rightmost lit bulb
        int rightmostLitIndex = -1;
        for (int i = healthBulbs.Count - 1; i >= 0; i--)
        {
            if (healthBulbs[i].IsLit)
            {
                rightmostLitIndex = i;
                break;
            }
        }

        // Update bar visibility
        for (int i = 0; i < healthBulbs.Count; i++)
        {
            if (healthBulbs[i].ConnectingBar != null)
            {
                // Show bar only if this bulb is lit AND it's not the rightmost lit bulb
                bool shouldShow = i < rightmostLitIndex && healthBulbs[i].IsLit;
                healthBulbs[i].ConnectingBar.SetActive(shouldShow);
            }
        }
    }
}