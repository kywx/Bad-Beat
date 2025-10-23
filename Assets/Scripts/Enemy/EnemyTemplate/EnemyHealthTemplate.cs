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

    public virtual void TakeDamage(float damage, Element attackType)
    {
        float damageMultiplier = 1f;

        if (attackType == _enemyStats.weakness) damageMultiplier *= _enemyStats.vulnerabilityModifier;
        if (attackType == _enemyStats.strength) damageMultiplier /= _enemyStats.resistanceModifier;
        
        _hp -= damage * damageMultiplier;

        if (_hp <= 0) Die();

    }

    public virtual void TakeDamageSimple(float damage) // use this until player attacks can provide an enum for elemental type
    {
        _hp -= damage;

        if (_hp <= 0) Die();
    }
    
    public virtual void Die()
    {
        //
    }
    
    
}
