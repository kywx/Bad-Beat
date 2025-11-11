using UnityEngine;

public class RatHealth : EnemyHealthTemplate
{
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void TakeDamageSimple(float damage)
    {
        //Apply knockback
        _hp -= damage;
        print("Damage to rat HP = " + _hp);

        if (_hp <= 0) 
        { 
            Die(); 
        } 
    }

    public override void Die()
    {
        //Play animation
        base.Die();
    }

}
