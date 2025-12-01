using UnityEngine;
using UnityEngine.UI;

public class CustomVolumeSlider : MonoBehaviour
{
    [Header("Audio Reference")]
    public AudioSource audioManager;

    [Header("Volume Sprites")]
    public Button[] volumeButtons = new Button[10];

    [Header("Sprite Settings")]
    public Sprite volumeBarSprite;
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    [Header("Size Settings")]
    public float baseScale = 1.0f;
    public float maxScale = 1.9f; // Grows to 190% at level 10

    private int currentLevel = 10; // Start at max volume

    [Header("Shake Icon")]
    public Transform shakeIcon;
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 10f;

    private Vector3 originalIconPosition;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        // Find Audio Manager if not assigned
        if (audioManager == null)
        {
            GameObject am = GameObject.Find("Audio Manager");
            if (am != null)
                audioManager = am.GetComponent<AudioSource>();
        }

        // Store original icon position
        if (shakeIcon != null)
        {
            originalIconPosition = shakeIcon.localPosition;
        }

        // Setup button listeners for each volume bar
        for (int i = 0; i < volumeButtons.Length; i++)
        {
            int level = i + 1; // Capture the level for this button (1-10)
            volumeButtons[i].onClick.AddListener(() => SetVolumeLevel(level));
        }

        // Load saved volume or use current
        if (audioManager != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            audioManager.volume = savedVolume;
            currentLevel = Mathf.RoundToInt(savedVolume * 10);
        }

        if (currentLevel == 0) currentLevel = 10; // Default to max if 0
    }

    private void OnEnable()
    {
        // Update display whenever the menu is opened
        UpdateVolumeDisplay();
    }

    public void SetVolumeLevel(int level)
    {
        currentLevel = level;
        UpdateVolumeDisplay();
        UpdateAudioVolume();

        // Trigger shake animation
        if (shakeIcon != null)
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(ShakeIcon());
        }
    }

    private void UpdateVolumeDisplay()
    {
        for (int i = 0; i < volumeButtons.Length; i++)
        {
            Image barImage = volumeButtons[i].GetComponent<Image>();

            if (i < currentLevel)
            {
                // Active sprite
                barImage.color = activeColor;
                barImage.sprite = volumeBarSprite;

                // Calculate scale: grows from baseScale to maxScale
                float scalePercent = (i + 1) / 10f;
                float scale = Mathf.Lerp(baseScale, maxScale, scalePercent);
                barImage.transform.localScale = new Vector3(1f, scale, 1f);
            }
            else
            {
                // Inactive sprite
                barImage.color = inactiveColor;
                barImage.sprite = volumeBarSprite;

                // Still maintain the progressive scale even when inactive
                float scalePercent = (i + 1) / 10f;
                float scale = Mathf.Lerp(baseScale, maxScale, scalePercent);
                barImage.transform.localScale = new Vector3(1f, scale, 1f);
            }
        }
    }

    private void UpdateAudioVolume()
    {
        float newVolume = currentLevel / 10f;

        // Use the singleton Audio Manager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(newVolume);
        }

        // Save the preference
        PlayerPrefs.SetFloat("MasterVolume", newVolume);
        PlayerPrefs.Save();
    }

    private System.Collections.IEnumerator ShakeIcon()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            shakeIcon.localPosition = originalIconPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Return to original position
        shakeIcon.localPosition = originalIconPosition;
    }
}