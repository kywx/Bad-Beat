using UnityEngine;

public class MiniBossProjectile1 : OneInstanceDamage
{
    [SerializeField]
    private float gravity = 0f;         // Can Tweaked if want non straight 
    [SerializeField]
    private float _y_velocity = 0f;
    [SerializeField]
    private float _x_velocity = 5f;
    [SerializeField]
    private float lifespan = 5f;

    private bool hit = false;

    void Update()
    {
        // Gravity update
        _y_velocity -= gravity * Time.deltaTime;

        // Lifespan decrease
        lifespan -= Time.deltaTime;

        // Death check
        if (lifespan <= 0 || hit)
        {
            Die();
        }

        // Move projectile
        Vector2 newPos = new Vector2(transform.position.x + _x_velocity * Time.deltaTime, transform.position.y + _y_velocity * Time.deltaTime);
        transform.position = newPos;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerHealth healthManager = collider.gameObject.GetComponentInParent<PlayerHealth>();
        if (healthManager != null && !hit)
        {
            hit = true;
            healthManager.Damage(damage_value);
            Die();
        }
        // Optionally: Add splash effect logic here!
    }

    public void SetDirection(Vector2 direction)
    {
        Vector2 normalizedDir = direction.normalized;
        float originalSpeed = new Vector2(_x_velocity, _y_velocity).magnitude;
        Vector2 newVelocity = normalizedDir * originalSpeed;
        _x_velocity = newVelocity.x;
        _y_velocity = newVelocity.y;
    }
}
