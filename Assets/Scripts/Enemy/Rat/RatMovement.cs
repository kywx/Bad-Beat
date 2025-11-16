using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class RatMovement : EnemyMovementTemplate
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform ray;
    bool facingRight = true;
    [SerializeField] private float stunTimer;
    private float stunCounter;
    private RatHealth health;
    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<RatHealth>();
    }

    protected override void RunPatrol()
    {
        if (stunCounter <= 0)
        {
            bool hit = Physics2D.Raycast(ray.position, Vector2.down, .5f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(ray.position, Vector2.down * 1f, Color.red);
            if (hit == true) {
                if (facingRight == true) {
                    rb.linearVelocity = new Vector2(_groundSpeed, rb.linearVelocityY);
                } else {
                    rb.linearVelocity = new Vector2(-_groundSpeed, rb.linearVelocityY);

                }
            } else {
                facingRight = !facingRight;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        } else
        {
            stunCounter -= Time.deltaTime;

        }

    }


    protected override void DetermineMovement()
    {
        if (health.IsAlive == false)
        {
            _movementType = EnemyMovement.Unique;
            rb.linearVelocity = Vector2.zero;
        }
        else if (_grounded == true)
        {
            _movementType = EnemyMovement.Patrol;
        }
        else if (_grounded == false)
        {
            _movementType = EnemyMovement.Idle;

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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
        stunCounter = stunTimer;


    }

}