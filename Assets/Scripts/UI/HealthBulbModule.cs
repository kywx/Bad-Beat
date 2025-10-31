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
    private Vector2 originalSize; // Store the lit bulb size

    public GameObject ConnectingBar { get; private set; }
    public bool IsLit => isLit;

    public void Initialize(int bulbIndex, Sprite lit, Sprite broken, GameObject connectingBar)
    {
        index = bulbIndex;
        litSprite = lit;
        brokenSprite = broken;
        ConnectingBar = connectingBar;

        rectTransform = GetComponent<RectTransform>();
        bulbImage = GetComponent<Image>();
        if (bulbImage == null)
            bulbImage = gameObject.AddComponent<Image>();

        // Set image properties - DON'T preserve aspect since sprites are different sizes
        bulbImage.preserveAspect = false;
        bulbImage.type = Image.Type.Simple;

        // Store the original size based on lit sprite
        if (litSprite != null)
        {
            originalSize = litSprite.rect.size;
            rectTransform.sizeDelta = originalSize;
        }

        animator = GetComponent<Animator>();
    }

    public void SetLit(bool lit)
    {
        isLit = lit;
        if (bulbImage != null)
        {
            bulbImage.sprite = lit ? litSprite : brokenSprite;

            // CRITICAL: Keep the same RectTransform size regardless of which sprite is shown
            // This prevents the broken sprite from appearing smaller
            if (rectTransform != null && originalSize != Vector2.zero)
            {
                rectTransform.sizeDelta = originalSize;
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
            // Fallback to code-based effect if no animator
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
            // Fallback to code-based effect
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
            float glow = Mathf.Sin(t * Mathf.PI * 3); // Multiple pulses
            bulbImage.color = Color.Lerp(originalColor, Color.yellow, glow * 0.5f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bulbImage.color = originalColor;
    }
}