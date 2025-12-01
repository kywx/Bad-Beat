using Unity.Mathematics;
using UnityEngine;

public class BossMinionMovement : EnemyMovementTemplate
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float turnBuffer; //prevents repeated turning in place.

    private ObstacleCheck obstacleCheck;
    private Animator animator;

    private RatHealth health;

    private SpriteRenderer spr;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        obstacleCheck = GetComponent<ObstacleCheck>();
        health = GetComponent<RatHealth>();
        spr = GetComponent<SpriteRenderer>();
    }

    protected override void RunIdle()
    {
        _idleCountdown -= Time.deltaTime;

        if(_idleCountdown <= 0)
        {
            _movementType = EnemyMovement.Chase;
        }
    }

    protected override void DetermineMovement()
    {
        if(_idleCountdown > 0)
        {
            _movementType = EnemyMovement.Idle;
        }
        else if (health.IsAlive == false)
        {
            if (_grounded == true)
            {
                Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    collider.enabled = false;
                }
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            rb.linearVelocity = Vector2.zero;
            _movementType = EnemyMovement.Idle;
        }
        else
        {
            RunChase();
        }
    }

    
    protected override void RunChase()
    {
        if (health.IsAlive == true){
            checkFlip();
            rb.linearVelocityX = _groundSpeed;
       } 
    }

    void checkFlip()
    {
        float _playerDistance = _player.transform.position.x - this.transform.position.x;
        if(_playerDistance * _groundSpeed < 0 && math.abs(_playerDistance) > turnBuffer)
        {
            _groundSpeed *= -1;
            spr.flipX = !spr.flipX;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.Damage(_enemyStats.attackDamage);

            animator.SetTrigger("PlayerPresent");
            rb.linearVelocity = Vector2.zero;
            _movementType = EnemyMovement.Idle;
            _idleCountdown = _idleTimer;
        }
    }
}