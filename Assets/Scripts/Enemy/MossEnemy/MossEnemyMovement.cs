using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class MossEnemyMovement: EnemyMovementTemplate
{
    
    private MossEnemyAttack _attackManager;
    private SpriteRenderer spr;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float sightWidth; //vertical range player can be in where moss guy will take the shot.

    [SerializeField]
    private float groundCheckDistance; //
    [SerializeField]
    private float shootDistance; //distance player can be in 

    [SerializeField]
    private bool fallOffPlatforms;
    private bool _isFacingRight;
    private float radius;
    protected override void Awake()
    {
        if(_patrolPoints.Count == 0)
            _patrolPoints = null;
        
        base.Awake();
        radius = this.transform.localScale.x/2;
        _isFacingRight = true;
        _attackManager = this.GetComponent<MossEnemyAttack>();
        spr = this.GetComponent<SpriteRenderer>();
    }

    protected override void DetermineMovement()
    {
        Vector3 playerPos = _player.transform.position;
        
        if(math.abs(playerPos.y - this.transform.position.y) < sightWidth
        && math.abs(playerPos.x - this.transform.position.x) < shootDistance)
        {
            _movementType = EnemyMovement.Unique;
        }

        else if (math.abs(playerPos.x - this.transform.position.x) < _startChaseDistance
            && math.abs(playerPos.y - this.transform.position.y) < 5)  // arbitrary number, please change
        {
            _movementType = EnemyMovement.Chase;
        }
        
        else
        {
            _movementType = EnemyMovement.Patrol;
        }
    }


    protected override void RunPatrol()
    {
        //walk back and forth
        AvoidFalling();
        rb.linearVelocityX = _groundSpeed;
        
        if (_grounded)
        {
            //Debug.Log("attempting jump");
            rb.linearVelocityY = 5;
        }
    }
    protected override void RunChase()
    {
        if(!AvoidFalling() && (_player.transform.position.x - this.transform.position.x) * _groundSpeed < 0)
        {
            Turn();
        }
        rb.linearVelocityX = _groundSpeed;

    }
    protected override void RunUnique()
    {
        if(_grounded){
            _attackManager.Shoot(_isFacingRight);
            rb.linearVelocityX = 0;
        }
        if((_player.transform.position.x - this.transform.position.x) * _groundSpeed < 0)
        {
            Turn();
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if(collision.gameObject.tag == "Wall")
        {
            Turn();
        }
    }
    private void Turn()
    {
        _isFacingRight = !_isFacingRight;
        _groundSpeed *= -1;
        spr.flipX = !spr.flipX;
        //Debug.Log(_grounded);
    }
    private bool AvoidFalling()
    {
        if(!fallOffPlatforms){
            float offset = radius * (_isFacingRight ? 1 : -1) * 2;
            
            Vector2 startingPoint = new Vector2(this.transform.position.x + offset, this.transform.position.y);
            bool aboveGround = Physics2D.Raycast(startingPoint, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
            if (!aboveGround)
            {
                Turn(); //turn to avoid falling off a platform
                return true;
            }
        }
        return false;
    }
}