using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class RatMovement : EnemyMovementTemplate
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform ray;
    [SerializeField] private float stunTimer;
    [SerializeField] private float attackTimer;
    [SerializeField] private float jumpForceX;
    [SerializeField] private float jumpForceY;

    private float stunCounter;
    private float attackCounter;
    private bool facingRight = true;

    private RatHealth health;
    private Animator animator;
    private ObstacleCheck obstacleCheck;
    private PlayerDetection playerDetection;




    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<RatHealth>();
        animator = GetComponent<Animator>();
        obstacleCheck = GetComponentInChildren<ObstacleCheck>();
        playerDetection = GetComponentInChildren<PlayerDetection>();
    }

    protected override void RunIdle()
    {
        //Do nothing 
    }

    protected override void RunPatrol()
    {        

        bool hit = Physics2D.Raycast(ray.position, Vector2.down, .5f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(ray.position, Vector2.down * .5f, Color.red);
        if (hit == true) {
            if (facingRight == true) {
                rb.linearVelocity = new Vector2(_groundSpeed, rb.linearVelocityY);
            } else {
                rb.linearVelocity = new Vector2(-_groundSpeed, rb.linearVelocityY);
            }
        } else {
            Flip();
        }

    }


    protected override void DetermineMovement()
    {
        if (obstacleCheck.obstacleDetected == true)
        {
            Flip();
        }

        if (StunCounter > 0)
        {
            StunCounter -= Time.deltaTime;
            _movementType = EnemyMovement.Idle;
        }
        else if (AttackCounter > 0)
        {
            AttackCounter -= Time.deltaTime;
            _movementType = EnemyMovement.Idle;
        }
        else if (health.IsAlive == false)
        {
            if (_grounded == true)
            {
                print("Disabling colliders and rigidbody");
                Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    collider.enabled = false;
                }
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
            _movementType = EnemyMovement.Idle;
        }
        else if (_grounded == false)
        {
            _movementType = EnemyMovement.Idle;

        }
        else if (playerDetection.playerDetected == true)
        {
            JumpTo(playerDetection.playerPosition);
        }
        else
        {
            _movementType = EnemyMovement.Patrol;
        }




    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _grounded = true;

        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _grounded = false;

        }
    }

    public override void Knockback(Vector2 attackerPosition, float knockbackStrength)
    {
        rb.linearVelocity = Vector2.zero;
        int dir;
        if (attackerPosition.x < this.transform.position.x) {
            dir = 1;
        } else if (attackerPosition.x > this.transform.position.x) {
            dir = -1;
        } else {
            dir = 0;
        }
        rb.linearVelocity = new Vector2(dir * knockbackStrength, knockbackStrength);
        StunCounter = stunTimer;


    }
    public float StunCounter
    {
        get
        {
            return stunCounter;
        }
        set
        {
            stunCounter = value;
            animator.SetFloat("stunCounter", value);
        }
    }
    protected void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    protected void JumpTo(Vector2 position)
    {
        rb.linearVelocity = Vector2.zero;
        int dir;
        if (position.x < this.transform.position.x)
        {
            dir = -1;
        }
        else 
        {
            dir = 1;
        }
        rb.linearVelocity = new Vector2(dir * jumpForceX, jumpForceY);
        AttackCounter = attackTimer;


    }
    public float AttackCounter
    {
        get
        {
            return attackCounter;
        }
        set
        {
            attackCounter = value;
            animator.SetFloat("attackCounter", value);
        }
    }
}