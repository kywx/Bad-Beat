using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected ProjectileDataSO data;
    protected int damage;
    protected Vector2 direction;
    protected int ownerLayer;
    protected bool hitSomething = false;
    
    // ===== (Component Cache) =====
    protected Rigidbody2D rb;
    protected Transform cachedTransform;
    protected Collider2D cachedCollider;
    protected SpriteRenderer spriteRenderer;
    protected TrailRenderer trail;
    
    // target for homing projectiles
    protected Transform target;
    protected float homingTimer;
    private bool isHomingEnabled;
    
    // pierce and bounce counters
    protected int pierceCount = 0;
    protected int bounceCount = 0;
    
    // cahced values for calculations
    private float speedSquared; // square of speed for optimization
    private Vector2 lastVelocity;

    protected virtual void Awake()
    {   
        // ===== componet cashe =====
        rb = GetComponent<Rigidbody2D>();
        cachedTransform = transform;
        cachedCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        
        // create Rigidbody2D if not exist
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }
    
    public virtual void Initialize(Vector2 dir, int dmg, ProjectileDataSO projectileData, int layer)
    {
        direction = dir.normalized;
        damage = dmg;
        data = projectileData;
        ownerLayer = layer;
        
        // physics setup
        if (rb != null)
        {
            rb.gravityScale = data.useGravity ? data.gravityScale : 0;
            rb.linearVelocity = direction * data.speed;
            lastVelocity = rb.linearVelocity;
            speedSquared = data.speed * data.speed; 
        }
        
        // dragging
        if (data.trailPrefab != null && trail == null)
        {
            trail = Instantiate(data.trailPrefab, cachedTransform);
        }
        
        // auto destroy after lifetime
        Destroy(gameObject, data.lifetime);
        
    
        if (data.isHoming)
        {
            Invoke(nameof(FindTarget), data.homingDelay);
        }
    }
    
    protected virtual void FixedUpdate()
    {   
         if (data == null || rb == null)
    {
        return; // Exit early if not initialized yet
    }
        // cache current velocity
        lastVelocity = rb.linearVelocity;
        // ===== SAFETY CHECK: Ensure data is initialized =====
    
        // target homing logic
        if (data.isHoming && target != null)
        {
            homingTimer += Time.fixedDeltaTime;
            if (homingTimer >= data.homingDelay)
            {
                // use cached transform
                Vector2 toTarget = (Vector2)target.position - (Vector2)cachedTransform.position;
                Vector2 targetDirection = toTarget.normalized;
                
                rb.linearVelocity = Vector2.Lerp(
                    rb.linearVelocity, 
                    targetDirection * data.speed, 
                    data.homingStrength * Time.fixedDeltaTime
                );
                
                // update rotation transform）
                float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
                cachedTransform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        ApplyMovementBehavior();
    
    }
    
   
    protected virtual void OnTriggerEnter2D(Collider2D collision)


    {    hitSomething = true; 
        // ignore self-collision
       
       
       if (collision.gameObject.layer == ownerLayer) 
    {
        return;
    }

    if (collision.GetComponent<ProjectileBase>() != null)
    {
        return; 
    }

    

        // CompareTag
        // the tag of the collided componet object  must be the label
      
       //  ===== damage enemy =====

        //must use GetComponenetInParent 
        EnemyHealthTemplate attackEnemy= collision.GetComponentInParent<EnemyHealthTemplate>();
        if (attackEnemy != null) {
                attackEnemy.TakeDamageSimple(damage);
                OnHit(collision);
                hitSomething = true; 
                Destroy(gameObject);
                
                //Debug.Log("Hit enemy with damge");
               
            } 


        // ===== damage Player =====
        PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {   
            playerHealth.Damage(damage);
            OnHit(collision);
            hitSomething = true; 
        }
    
    
       
 
            if (data.bouncing && bounceCount < data.maxBounceCount)
            {
                Bounce(collision);
                bounceCount++;
                return;
            }
            if (hitSomething)
            {
                    if (data.piercing && pierceCount < data.maxPierceCount)
                {
                    pierceCount++;
                    OnHit(collision);
                    return;
                }
                    DestroyProjectile();    
            }
            
        
    }
    
    protected virtual void OnHit(Collider2D collision)
    {
        // use cached position
        Vector3 hitPosition = cachedTransform.position;
        
        // hit effects if any
        if (data.hitEffect != null)
        {
            Instantiate(data.hitEffect, hitPosition, Quaternion.identity);
        }
        
        if (data.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(data.hitSound, hitPosition);
        }
    }
    
    protected virtual void Bounce(Collider2D collision)
    {
        // cached location lastVelocity
        Vector2 contactPoint = collision.ClosestPoint(cachedTransform.position);
        Vector2 normal = ((Vector2)cachedTransform.position - contactPoint).normalized;
        
        // use cached velocity to calculate reflection
        Vector2 reflect = Vector2.Reflect(lastVelocity, normal);
        rb.linearVelocity = reflect;
        
        // update rotation
        float angle = Mathf.Atan2(reflect.y, reflect.x) * Mathf.Rad2Deg;
        cachedTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    protected virtual void FindTarget()
    {
        //  FindGameObjectsWithTag（search all at one）
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(data.targetTag);
        
        if (enemies.Length == 0) return;
        
        float closestDistanceSqr = Mathf.Infinity;
        Transform closestTarget = null;
        Vector2 currentPos = cachedTransform.position;
        
        // use cached squared speed for optimization
        foreach (GameObject enemy in enemies)
        {
            Vector2 offset = enemy.transform.position - (Vector3)currentPos;
            float distanceSqr = offset.sqrMagnitude;
            
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestTarget = enemy.transform;
            }
        }
        
        target = closestTarget;
    }
    
    protected virtual void DestroyProjectile()
    {
        
        if (data.destroyEffect != null)
        {   // use cached position
            Instantiate(data.destroyEffect, cachedTransform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    protected virtual void ApplyMovementBehavior()
        {
            // Empty - meant to be overridden by subclasses
        }

}
