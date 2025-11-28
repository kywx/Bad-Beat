using UnityEngine;

public class MossProjectile : OneInstanceDamage
{
    [SerializeField]
    private float gravity;

    [SerializeField]
    private float _y_velocity;
    [SerializeField]
    private float _x_velocity;

    [SerializeField]
    private float lifespan;

    private bool hit = false;
    private void Update()
    {
        _y_velocity -= gravity * Time.deltaTime;
        lifespan -= Time.deltaTime;
        bool checkGround = Physics2D.Raycast(this.transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if(lifespan <= 0 || checkGround)
        {
            die();
        }
        Vector2 newPos = new Vector2(this.transform.position.x + _x_velocity * Time.deltaTime, this.transform.position.y + _y_velocity*Time.deltaTime);
        this.transform.position = newPos;
    }
    private void die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerHealth healthManager = collider.gameObject.GetComponentInParent<PlayerHealth>();
        if(healthManager != null && !hit)
        {
            hit = true;
            //Debug.Log("hit");
            healthManager.Damage(damage_value);       
            die();
        }
    }
    public void setDirection(bool goRight)
    {
        _x_velocity = _x_velocity * (goRight ? 1 : -1);
    }
}