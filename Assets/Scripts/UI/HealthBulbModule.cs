using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Individual health bulb component
/// </summary>
public class HealthBulbModule : MonoBehaviour
{
    private Image bulbImage;
    private Animator animator;
    private Sprite litSprite;
    private Sprite brokenSprite;
    private int index;
    private bool isLit;
    private RectTransform rectTransform;
    private Vector2 fixedSize; // Use a consistent size for both states

    public GameObject ConnectingBar { get; private set; }
    public bool IsLit => isLit;

    public void Initialize(int bulbIndex, Sprite lit, Sprite broken, GameObject connectingBar, Vector2 size)
    {
        index = bulbIndex;
        litSprite = lit;
        brokenSprite = broken;
        ConnectingBar = connectingBar;
        fixedSize = size;

        rectTransform = GetComponent<RectTransform>();
        bulbImage = GetComponent<Image>();
        if (bulbImage == null)
            bulbImage = gameObject.AddComponent<Image>();

        // CRITICAL: Don't preserve aspect - we control size manually
        bulbImage.preserveAspect = false;
        bulbImage.type = Image.Type.Simple;

        // Disable raycast target if not needed for performance
        bulbImage.raycastTarget = false;

        // CRITICAL: Enforce size again to be absolutely sure
        rectTransform.sizeDelta = fixedSize;

        animator = GetComponent<Animator>();
    }

    public void SetLit(bool lit)
    {
        isLit = lit;
        if (bulbImage != null)
        {
            bulbImage.sprite = lit ? litSprite : brokenSprite;

            // CRITICAL: Always maintain the same RectTransform size
            // This ensures both sprites display at the same size
            if (rectTransform != null && fixedSize != Vector2.zero)
            {
                rectTransform.sizeDelta = fixedSize;
            }
        }
    }

    public void PlayExplosionAnimation()
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger("Explode");
        }
        else
        {
            StartCoroutine(ShakeEffect());
        }
    }

    public void PlayHealAnimation()
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger("Heal");
        }
        else
        {
            StartCoroutine(GlowEffect());
        }
    }

    private IEnumerator ShakeEffect()
    {
        Vector3 originalPos = transform.localPosition;
        float duration = 0.3f;
        float elapsed = 0f;
        float intensity = 5f;

        while (elapsed < duration)
        {
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);
            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private IEnumerator GlowEffect()
    {
        if (bulbImage == null) yield break;

        Color originalColor = bulbImage.color;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float glow = Mathf.Sin(t * Mathf.PI * 3);
            bulbImage.color = Color.Lerp(originalColor, Color.yellow, glow * 0.5f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bulbImage.color = originalColor;
    }

    /// <summary>
    /// Get the top Y position of this bulb in local space
    /// </summary>
    public float GetTopYPosition()
    {
        return rectTransform.anchoredPosition.y + (rectTransform.sizeDelta.y / 2f);
    }
}