using UnityEngine;

public class MiniBossProjectile : OneInstanceDamage
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

    public void setDirection(bool goRight)
    {
        _x_velocity = Mathf.Abs(_x_velocity) * (goRight ? 1 : -1);
    }
}
