using UnityEngine;

public abstract class EnemyTemplate : MonoBehaviour
{
    [SerializeField] protected EnemyDefaultStats _enemyStats;

    // general variables all enemies need when initialized
    protected float _hp;
    protected float _attackCooldown; // this will tick down and be set equal to the SO's cooldown when it's <= 0


    protected virtual void Awake()
    {
        _hp = _enemyStats.hp; // starting health
        _attackCooldown = _enemyStats.attackCooldown; // maybe redundant

    }
    
    
}
