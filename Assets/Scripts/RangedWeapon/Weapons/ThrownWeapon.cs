using UnityEngine;

public class ThrownWeapon : WeaponBase
{
    [Header("Throwing Settings")]
    public float throwAngle = 30f;
    public int projectileCount = 1;
    public float spreadAngle = 0f;
    
    // ===== cache componet =====
    private Transform attackPointCache;
    
    protected override void PerformAttack(Vector2 direction, Transform attackPoint)
    {
        // attackPoint
        if (attackPointCache == null)
            attackPointCache = attackPoint;
        throwAngle = weaponData.throwAngle;
        projectileCount = weaponData.projectileCount;
        spreadAngle = weaponData.spreadAngle;
        // single projectile
        if (projectileCount == 1)
        {
            SpawnProjectile(direction, attackPointCache);
        }
        // area projectiles (spread)
        else
        {
            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / (projectileCount - 1);
            
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                Vector2 spreadDirection = RotateVector(direction, angle);
                SpawnProjectile(spreadDirection, attackPointCache);
            }
        }
    }
    
   void SpawnProjectile(Vector2 direction, Transform attackPoint)
{
    // Spawn projectile
    GameObject projectileObj = Instantiate(
        weaponData.projectilePrefab, 
        attackPoint.position, 
        Quaternion.identity
    );
    
    // Set rotation FIRST (before Initialize sets velocity)
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    projectileObj.transform.rotation = Quaternion.Euler(0, 0, angle);


    // Initialize IMMEDIATELY (before any Update/FixedUpdate runs)
    ProjectileBase projectile = projectileObj.GetComponent<ProjectileBase>();
    
    if (projectile != null)
    {
        projectile.Initialize(
            direction, 
            weaponData.damage, 
            weaponData.projectileData,
            gameObject.layer
        );
    }
    else
    {
        Debug.LogError("Projectile prefab is missing ProjectileBase component!");
        Destroy(projectileObj);
    }
}

    
    Vector2 RotateVector(Vector2 v, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
