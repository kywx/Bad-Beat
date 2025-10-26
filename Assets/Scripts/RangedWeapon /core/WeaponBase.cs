using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [Header("Weapon Configuration")]
    public WeaponDataSO weaponData;
    
    protected float lastAttackTime = -Mathf.Infinity; 
    
    // ===== Component Cache =====
    protected Transform cachedTransform;
    protected AudioSource audioSource;
    
    protected virtual void Awake()
    {
        cachedTransform = transform;
        
        
        // ===== FIX: Don't access weaponData in Awake - it's not assigned yet =====
        // Audio source will be created when needed in PlayEffects()
        audioSource = GetComponent<AudioSource>();
    }
    
    public virtual void Attack(Vector2 direction, Transform attackPoint)
    {   
        if (!CanAttack()) return;
        
        // Consume resources
        if (!ConsumeResources()) return;
        
        // Perform attack
        PerformAttack(direction, attackPoint);
        
        // Play effects
        PlayEffects(attackPoint.position);
        
        // Record attack time
        
        lastAttackTime = Time.time;
    }
    
    protected abstract void PerformAttack(Vector2 direction, Transform attackPoint);
    
    public virtual bool CanAttack()
    {
        return Time.time >= lastAttackTime + weaponData.cooldownTime;
    }
    
    protected virtual bool ConsumeResources()
    {
        // maybe mana cost
        
        return true;
    }
    
    protected virtual void PlayEffects(Vector3 position)
    {
        // ===== Create AudioSource on demand if needed =====
        if (weaponData.attackSound != null)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
            audioSource.PlayOneShot(weaponData.attackSound);
        }
        
        if (weaponData.attackEffect != null)
        {
            Instantiate(weaponData.attackEffect, position, Quaternion.identity);
        }
    }
    
    public virtual void OnEquip()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void OnUnequip()
    {
        gameObject.SetActive(false);
    }
}
