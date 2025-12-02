using UnityEngine;

public class BossHealth : EnemyHealthTemplate
{   public GameObject keyPrefab; // Assign the key prefab in Inspector

     private Rigidbody2D rb;
    private bool isAlive = true;
    private Animator animator;
    private FlashHit flash;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flash = GetComponent<FlashHit>();
    }
    public override void TakeDamageSimple(float damage)
    {
        _hp -= damage;
        print("Damage to rat HP = " + _hp);
        flash.Flash();

        if (_hp <= 0 && IsAlive) 
        { 
            IsAlive = false;
            //Animation condition IsAlive == false, will start the fadeTimer in fadeOut script
            //Destroy(gameObject) will be called when fadeTimer == 0. 

        }
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


    public override void Die()
    {
        // Spawn the key at boss position
        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
    
}