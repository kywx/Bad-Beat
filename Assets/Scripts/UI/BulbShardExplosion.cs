using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles the shard explosion effect when a bulb breaks
/// </summary>
public class BulbShardExplosion : MonoBehaviour
{
    [Header("Shard Settings")]
    [Tooltip("Sprite to use for individual shards")]
    public Sprite shardSprite;

    [Tooltip("Number of shards to spawn")]
    [Range(3, 20)]
    public int shardCount = 8;

    [Tooltip("Delay before shards spawn (allows bulb shake to finish)")]
    [Range(0f, 2f)]
    public float shardSpawnDelay = 0.3f;

    [Tooltip("How far shards can travel")]
    public Vector2 explosionForceRange = new Vector2(50f, 150f);

    [Header("Shard Lifetime")]
    [Tooltip("Minimum time before shard starts fading")]
    public float minFadeStartTime = 0.3f;

    [Tooltip("Maximum time before shard starts fading")]
    public float maxFadeStartTime = 0.8f;

    [Tooltip("How long the fade out takes")]
    public float fadeDuration = 0.4f;

    [Header("Physics")]
    [Tooltip("Gravity affecting shards (downward pull)")]
    public float gravity = 200f;

    [Tooltip("Random rotation speed range")]
    public Vector2 rotationSpeedRange = new Vector2(-360f, 360f);

    [Header("Shard Appearance")]
    [Tooltip("Size range for shards")]
    public Vector2 shardSizeRange = new Vector2(10f, 25f);

    [Tooltip("Color tint for shards (leave white for no tint)")]
    public Color shardTint = Color.white;

    /// <summary>
    /// Spawns an explosion of shards at the given position after a delay
    /// </summary>
    public void SpawnShardExplosion(Vector3 worldPosition, Transform parentCanvas)
    {
        StartCoroutine(SpawnShardExplosionDelayed(worldPosition, parentCanvas));
    }

    private IEnumerator SpawnShardExplosionDelayed(Vector3 worldPosition, Transform parentCanvas)
    {
        yield return new WaitForSeconds(shardSpawnDelay);

        for (int i = 0; i < shardCount; i++)
        {
            CreateShard(worldPosition, parentCanvas);
        }
    }

    private void CreateShard(Vector3 spawnPosition, Transform parentCanvas)
    {
        GameObject shardObj = new GameObject("BulbShard");
        shardObj.transform.SetParent(parentCanvas, false);

        RectTransform rect = shardObj.AddComponent<RectTransform>();
        rect.position = spawnPosition;

        // Random size for variety
        float size = Random.Range(shardSizeRange.x, shardSizeRange.y);
        rect.sizeDelta = new Vector2(size, size);

        // Add image component
        Image img = shardObj.AddComponent<Image>();
        img.sprite = shardSprite;
        img.color = shardTint;
        img.raycastTarget = false;

        // Random initial rotation
        rect.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        // Start the shard animation
        ShardBehavior behavior = shardObj.AddComponent<ShardBehavior>();
        behavior.Initialize(
            GetRandomExplosionDirection(),
            Random.Range(rotationSpeedRange.x, rotationSpeedRange.y),
            gravity,
            Random.Range(minFadeStartTime, maxFadeStartTime),
            fadeDuration
        );
    }

    private Vector2 GetRandomExplosionDirection()
    {
        // Random angle in all directions
        float angle = Random.Range(0f, 360f);
        float force = Random.Range(explosionForceRange.x, explosionForceRange.y);

        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * force,
            Mathf.Sin(angle * Mathf.Deg2Rad) * force
        );
    }
}

/// <summary>
/// Controls individual shard behavior - velocity, rotation, and fading
/// </summary>
public class ShardBehavior : MonoBehaviour
{
    private Vector2 velocity;
    private float rotationSpeed;
    private float gravity;
    private float fadeStartTime;
    private float fadeDuration;
    private float spawnTime;
    private Image image;
    private RectTransform rectTransform;
    private bool isFading = false;
    private float fadeStartedTime;

    public void Initialize(Vector2 initialVelocity, float rotSpeed, float grav, float fadeStart, float fadeDur)
    {
        velocity = initialVelocity;
        rotationSpeed = rotSpeed;
        gravity = grav;
        fadeStartTime = fadeStart;
        fadeDuration = fadeDur;
        spawnTime = Time.time;

        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rectTransform == null) return;

        float elapsed = Time.time - spawnTime;

        // Apply gravity to velocity
        velocity.y -= gravity * Time.deltaTime;

        // Move the shard
        rectTransform.anchoredPosition += velocity * Time.deltaTime;

        // Rotate the shard
        rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Handle fading
        if (!isFading && elapsed >= fadeStartTime)
        {
            isFading = true;
            fadeStartedTime = Time.time;
        }

        if (isFading && image != null)
        {
            float fadeElapsed = Time.time - fadeStartedTime;
            float alpha = 1f - (fadeElapsed / fadeDuration);

            if (alpha <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                Color col = image.color;
                col.a = alpha;
                image.color = col;
            }
        }
    }
}