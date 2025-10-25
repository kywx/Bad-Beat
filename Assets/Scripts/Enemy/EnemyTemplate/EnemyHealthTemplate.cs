using UnityEngine;

public abstract class EnemyHealthTemplate : MonoBehaviour
{
    [SerializeField] protected EnemyDefaultStats _enemyStats;

    // general health variables all enemies may want when initialized
    protected float _hp;
    protected int _armor;
    

    protected virtual void Awake()
    {
        _hp = _enemyStats.hp; // starting health
        _armor = _enemyStats.armor; // starting armor

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
        Destroy(gameObject);
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Trap") // change the string if the tag is wrong
        {
            Die();
        }
        
    }
    
    
}
