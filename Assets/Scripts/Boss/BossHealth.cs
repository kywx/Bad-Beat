using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class BossHealth : EnemyHealthTemplate
{   public GameObject keyPrefab; // Assign the key prefab in Inspector

     private Rigidbody2D rb;
    private bool isAlive = true;
    private Animator animator;
    private FlashHit flash;

    [SerializeField] private List<AudioResource> bossTracks;

    public int phase = 0;
    private bool isAsleep;

    private float maxHp;

    private MiniBossRangedAttack1 minibossRanged;
    private BossMovement bossMovement;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flash = GetComponent<FlashHit>();
        animator = GetComponent<Animator>();

        isAsleep = true;
        maxHp = _hp;

        minibossRanged = GetComponent<MiniBossRangedAttack1>();
        bossMovement = GetComponent<BossMovement>();

        bossMovement.enabled = false;
    }
    public override void TakeDamageSimple(float damage)
    {
        _hp -= damage;
        print("Damage to boss. HP = " + _hp);
        flash.Flash();

        if (_hp <= 0 && IsAlive) 
        { 
            IsAlive = false;
            //Animation condition IsAlive == false, will start the fadeTimer in fadeOut script
            //Destroy(gameObject) will be called when fadeTimer == 0. 

            animator.SetTrigger("die");

        }

        if (_hp < maxHp * 0.667  && phase == 0) // boss at 66% health
        {
            phase++;

            GetComponent<BossMinionSpawner>().enabled = false; 

            animator.SetInteger("phase", phase);

            Conductor.instance.ChangeMusic(bossTracks[phase]);

                   
        }

        if (_hp < maxHp * 0.333  && phase == 1) // boss at 33% health
        {
            phase++;

            minibossRanged._canShootSingle = false; // disables second attack

            animator.SetInteger("phase", phase);

            Conductor.instance.ChangeMusic(bossTracks[phase]);
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


    public override void Die() // Joseph will run this method through an animation event
    {
        // Spawn the key at boss position
        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAsleep)
        {
            animator.SetTrigger("intro");

            //isAsleep = false;
            Conductor.instance.ChangeMusic(bossTracks[0]);
        }
    }

    public void WakeUp() // for an animation event
    {
        isAsleep = false;
        animator.SetBool("isAsleep", isAsleep);
        bossMovement.enabled = true;
    }

}