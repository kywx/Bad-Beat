using UnityEngine;

/// <summary>
/// Fireball projectile - inherits from ProjectileBase
/// Adds rotation effect and particle system support
/// </summary>
public class Fireball : ProjectileBase
{
    [Header("Fireball Specific")]
    [Tooltip("Rotation speed in degrees per second")]
    public float rotationSpeed = 360f;
    
    // ===== Component Cache =====
    private ParticleSystem fireParticles;
    
    // Pre-calculated rotation per frame
    private float rotationDelta;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Cache particle system if exists
        fireParticles = GetComponent<ParticleSystem>();
        if (fireParticles != null)
        {
            fireParticles.Play();
        }
    }
    
    public override void Initialize(Vector2 dir, int dmg, ProjectileDataSO projectileData, int layer)
    {
        base.Initialize(dir, dmg, projectileData, layer);
        
        // Pre-calculate rotation delta to avoid calculating every frame
        rotationDelta = rotationSpeed * Time.fixedDeltaTime;
    }
    
    protected override void ApplyMovementBehavior()
    {
        // Rotate the fireball sprite continuously
        if (cachedTransform != null)
        {
            cachedTransform.Rotate(0, 0, rotationDelta);
        }
    }
    
    protected override void OnHit(Collider2D collision)
    {
        base.OnHit(collision);
        
        // Add any fireball-specific hit effects here
        // For example: ignite enemies, light torches, melt ice, etc.
    }
    
    protected override void DestroyProjectile()
    {
        // Stop particle emission but let existing particles finish
        if (fireParticles != null)
        {
            fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        
        base.DestroyProjectile();
    }
}
