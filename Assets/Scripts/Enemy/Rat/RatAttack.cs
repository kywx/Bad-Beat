using UnityEngine;

public class RatAttack : EnemyAttackTemplate
{
    private RatHealth myhealth;
    protected override void Awake()
    {
        base.Awake();
        myhealth = GetComponent<RatHealth>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

        if (health != null && myhealth.IsAlive == true)
        {
            //health.Damage(_attackDamage);

            //print("Damage to player by rat");
        }
    }


    protected void OnCollisionStay2D(Collision2D collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

        if (health != null && myhealth.IsAlive == true)
        {
            health.Knockback(transform.position, _enemyStats.knockbackStrength);
            health.Damage(_attackDamage);

            print("Damage/Knockback to player by rat");
        }
    }

}
