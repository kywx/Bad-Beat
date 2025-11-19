using UnityEngine;

/// <summary>
/// Weapon data configuration - Controls weapon stats and behavior
/// Used for all weapon types (thrown, shot, charged, etc.)
/// </summary>
[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapons/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("=== Basic Info ===")]
    [Tooltip("Weapon display name")]
    public string weaponName = "Weapon";
    
    [Tooltip("Weapon icon for UI")]
    public Sprite weaponIcon;
    
    [Tooltip("Weapon type - determines behavior class")]
    public WeaponType weaponType = WeaponType.Thrown;
    
    [Space(10)]
    [Header("=== Attack Stats ===")]
    [Tooltip("Base damage per hit")]
    public int damage = 1;
    
    [Tooltip("Attack speed (attacks per second)")]
    [Range(0.1f, 10f)]
    public float attackSpeed = 1f;
    
    [Tooltip("Cooldown time between attacks (seconds)")]
    [Range(0.1f, 5f)]
    public float cooldownTime = 0.5f;
    

    
    [Space(10)]
    [Header("=== Projectile Configuration ===")]
    [Tooltip("Projectile prefab to spawn")]
    public GameObject projectilePrefab;
    
    [Tooltip("Projectile data (speed, lifetime, behavior, etc.)")]
    public ProjectileDataSO projectileData;
    
    [Space(10)]
    [Header("=== Effects ===")]
    [Tooltip("Sound played when attacking")]
    public AudioClip attackSound;
    
    [Tooltip("Visual effect spawned at attack point")]
    public GameObject attackEffect;



    [Header("=== Throwing Configuration(Only for Thrown type) ===")]
    [Tooltip("Upward angle for arc trajectory (degrees)")]
    [Range(0f, 90f)]
    public float throwAngle = 30f;

    [Tooltip("Number of projectiles per attack")]
    [Range(1, 10)]
    public int projectileCount = 1;

    [Tooltip("Spread angle for multiple projectiles (degrees)")]
    [Range(0f, 180f)]
    public float spreadAngle = 0f;

    

}

/// <summary>
/// Weapon type enumeration - determines which weapon class to use
/// </summary>
public enum WeaponType
{
    Thrown,      // Thrown weapons (fireball, shuriken, knife)
    Shot,        // Shot weapons (bow, gun) - for future use
    Charged,     // Charged weapons (charged bow, magic) - for future use
    Spread,      // Spread weapons (shotgun) - for future use
    Beam,        // Beam weapons (laser) - for future use
    Homing       // Homing weapons (homing missiles) - for future use
}
