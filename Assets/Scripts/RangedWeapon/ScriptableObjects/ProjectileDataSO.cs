using UnityEngine;

/// <summary>
/// Projectile data configuration - Controls all projectile behavior and properties
/// Used for fireballs, arrows, bullets, etc.
/// </summary>
[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Weapons/Projectile Data")]
public class ProjectileDataSO : ScriptableObject
{
    [Header("=== Basic Info ===")]
    [Tooltip("Projectile name (for debugging and identification)")]
    public string projectileName = "Projectile";
    
    [Space(10)]
    [Header("=== Movement Properties ===")]
    [Tooltip("Projectile flight speed")]
    [Range(1f, 50f)]
    public float speed = 15f;
    
    [Tooltip("Projectile lifetime in seconds - Auto-destroy after this time")]
    [Range(0.5f, 10f)]
    public float lifetime = 3f;
    
    [Tooltip("Whether projectile is affected by gravity (arc trajectory)")]
    public bool useGravity = false;
    
    [Tooltip("Gravity scale (only active when useGravity = true)")]
    [Range(0f, 5f)]
    public float gravityScale = 1f;
    
    [Space(10)]
    [Header("=== Collision Behavior ===")]
    [Tooltip("Can projectile pierce through enemies")]
    public bool piercing = false;
    
    [Tooltip("Maximum pierce count (only active when piercing = true)")]
    [Range(1, 10)]
    public int maxPierceCount = 1;
    
    [Tooltip("Can projectile bounce off walls")]
    public bool bouncing = false;
    
    [Tooltip("Maximum bounce count (only active when bouncing = true)")]
    [Range(0, 5)]
    public int maxBounceCount = 1;
    
    [Space(10)]
    [Header("=== Homing System ===")]
    [Tooltip("Does projectile track targets")]
    public bool isHoming = false;
    
    [Tooltip("Homing strength (turning speed)")]
    [Range(0f, 20f)]
    public float homingStrength = 5f;
    
    [Tooltip("Delay before homing starts (seconds after launch)")]
    [Range(0f, 2f)]
    public float homingDelay = 0.2f;
    
    [Tooltip("Target tag to track (e.g., 'Enemy')")]
    public string targetTag = "Enemy";
    
    [Space(10)]
    [Header("=== Visual Effects ===")]
    [Tooltip("Effect spawned when projectile hits something")]
    public GameObject hitEffect;
    
    [Tooltip("Effect spawned when projectile is destroyed")]
    public GameObject destroyEffect;
    
    [Tooltip("Trail renderer prefab (optional)")]
    public TrailRenderer trailPrefab;
    
    [Space(10)]
    [Header("=== Audio ===")]
    [Tooltip("Sound played when projectile hits target")]
    public AudioClip hitSound;
}
