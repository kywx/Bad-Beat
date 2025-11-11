using UnityEngine;
using UnityEngine.UIElements;

public class RatMovement : EnemyMovementTemplate
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform ray;
    bool facingRight = true;
    
    protected override void RunPatrol()
    {
        bool hit = Physics2D.Raycast(ray.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
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

    }

    protected override void RunIdle()
    {
        base.RunIdle();
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
    }


    protected override void DetermineMovement()
    {
        if (_grounded == true)
        {
            _movementType = EnemyMovement.Patrol;
        }
        else {
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



}