using UnityEngine;

public class RatHealth : EnemyHealthTemplate
{
    private Rigidbody2D rb;
    private bool isAlive = true;
    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public override void TakeDamageSimple(float damage)
    {
        _hp -= damage;
        print("Damage to rat HP = " + _hp);

        if (_hp <= 0 && IsAlive) 
        { 
            IsAlive = false;
            Die(); 
        } 
    }

    public override void Die()
    {
        print("Disbling colliders and rigidbody");
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        rb.bodyType = RigidbodyType2D.Kinematic;

        //Destroy(gameObject)
        //Called when fadeTimer == 0 in fadeout script.
    }

    public bool IsAlive{
        get
        {  
            return isAlive; 
        }
        set
        {
            isAlive = value;
            animator.SetBool("isAlive", value);
            print("isAlive " + isAlive);
        }
    }

}
