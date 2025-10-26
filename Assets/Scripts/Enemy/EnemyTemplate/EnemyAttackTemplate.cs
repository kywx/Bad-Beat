using UnityEngine;

[System.Serializable]
    public enum AttackState
    {
        Melee,
        Ranged
    }
public abstract class EnemyAttackTemplate : MonoBehaviour
{
    [SerializeField] protected EnemyDefaultStats _enemyStats;
    [SerializeField] public AttackState _attackState { get; protected set; } // public get so that other scripts can know if the enemy is ranged or not, which could affect movement as an example

    protected float _minAttackRange; // may be redundant to have two ranges. Change if needed
    protected float _maxAttackRange; // the idea is if an enemy wants to "throw" a projectile that it will do so within the two ranges rather than throw the projectile immediately in front of them

    protected float _attackCooldown;
    protected int _attackDamage;

    [SerializeField] protected Transform _attackStart;
    [SerializeField] protected Transform _attackEnd;
    [SerializeField] protected float _attackSize;
    [SerializeField] protected LayerMask _attackLayer;

    protected void Awake()
    {
        _minAttackRange = _enemyStats.minAttackRange;
        _maxAttackRange = _enemyStats.maxAttackRange;
        _attackCooldown = _enemyStats.attackCooldown;
        _attackDamage = _enemyStats.attackDamage;
    }

    protected virtual void MeleeAttack() // use as an animation event
    {
        Vector2 attackDirection = (_attackEnd.position - _attackStart.position).normalized;
        float maxDistance = Vector2.Distance(_attackStart.position, _attackEnd.position);

        RaycastHit2D hit = Physics2D.CircleCast(_attackStart.position, _attackSize, attackDirection, maxDistance, _attackLayer);

        if(hit.collider.gameObject.tag == "Player")
        {
            //  hit.collider.gameObject.GetComponent<PLAYERLIFE>.TAKEDAMAGE(_attackDamage);
            //  hit.collider.gameObject.GetComponent<PLAYER____>.TAKEKNOCKBACK(_attackDamage);
            Debug.Log("Player was hit");
        }
    }

    protected virtual void RangedAttack()
    {
        //
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // implement when we have scripts managing the player's health
            //  collision.gameObject.GetComponent<PLAYERLIFE>.TAKEDAMAGE(_attackDamage);
            //  collision.gameObject.GetComponent<PLAYER____>.TAKEKNOCKBACK(_attackDamage);

            Debug.Log("Collision damage");
        }
    }




}
