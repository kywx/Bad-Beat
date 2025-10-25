using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon System")]
    public List<WeaponDataSO> availableWeapons = new List<WeaponDataSO>();
    public Transform attackPoint;
    public SpriteRenderer handSpriteRenderer;
    
    // ===== cached component  =====
    private Transform cachedTransform;
    private Camera mainCamera;
    
    // weapon dictionary
    private Dictionary<WeaponDataSO, WeaponBase> weaponInstances = new Dictionary<WeaponDataSO, WeaponBase>();
    private WeaponBase currentWeapon;
    private int currentWeaponIndex = 0;
    
    // cached variables
    private Vector3 mouseWorldPos;
    private Vector2 attackDirection;
    
    void Awake()
    {
        // cache components
        cachedTransform = transform;
        mainCamera = Camera.main; // cache main camera citation
    }
    
    void Start()
    {
        // inicialize weapon instances
        foreach (var weaponData in availableWeapons)
        {
            WeaponBase weaponInstance = CreateWeaponInstance(weaponData);
            if (weaponInstance != null)
            {
                weaponInstances[weaponData] = weaponInstance;
                weaponInstance.OnUnequip();
            }
        }
        
        // equip first weapon by default
        if (availableWeapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }
    
    void Update()
    {
        // attack input
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
        
        // siwtch weapon input
        HandleWeaponSwitching();
    }
    
    void Attack()
    {
        if (currentWeapon != null)
        {
            // calculate attack direction
            attackDirection = GetAttackDirection();
            currentWeapon.Attack(attackDirection, attackPoint);
        }
    }
    
    Vector2 GetAttackDirection()
    {
        // user 2D mouse position to world position
        mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // calculate direction vector
        attackDirection.x = mouseWorldPos.x - attackPoint.position.x;
        attackDirection.y = mouseWorldPos.y - attackPoint.position.y;
        
        // normalize direction
        float magnitude = Mathf.Sqrt(attackDirection.x * attackDirection.x + 
                                     attackDirection.y * attackDirection.y);
        
        if (magnitude > 0.0001f)
        {
            attackDirection.x /= magnitude;
            attackDirection.y /= magnitude;
        }
        
        return attackDirection;
    }
    
    void HandleWeaponSwitching()
    {
        // Q/E switch
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon(1);
        }
        
        // number keys switch
        if (Input.GetKeyDown(KeyCode.Alpha1) && availableWeapons.Count > 0)
            EquipWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && availableWeapons.Count > 1)
            EquipWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && availableWeapons.Count > 2)
            EquipWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4) && availableWeapons.Count > 3)
            EquipWeapon(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5) && availableWeapons.Count > 4)
            EquipWeapon(4);
    }
    
    void SwitchWeapon(int direction)
    {
        currentWeaponIndex += direction;
        
        int weaponCount = availableWeapons.Count;
        currentWeaponIndex = (currentWeaponIndex + weaponCount) % weaponCount;
        
        EquipWeapon(currentWeaponIndex);
        
    }
    
    void EquipWeapon(int index)
    {
        if (index < 0 || index >= availableWeapons.Count) return;
        
        // unequip current weapon
        currentWeapon?.OnUnequip();
        
        // equip new weapon
        currentWeaponIndex = index;
        WeaponDataSO weaponData = availableWeapons[index];
        
        if (weaponInstances.TryGetValue(weaponData, out WeaponBase weapon))
        {
            currentWeapon = weapon;
            currentWeapon.OnEquip();
            UpdateHandVisual(weaponData);
        }
    }

    

            // Called when you switch weapons
    public void UpdateHandVisual(WeaponDataSO weaponData)
        {
            if (handSpriteRenderer == null) return;

                handSpriteRenderer.sprite = weaponData.weaponIcon;


                 Transform handTransform = handSpriteRenderer.transform;
                 handTransform.localScale = new Vector3(0.005f, 0.005f, 0f);
                
                
            // handSpriteRenderer could be at the attaackPoint position
        }

    
    WeaponBase CreateWeaponInstance(WeaponDataSO weaponData)
    {
        WeaponBase weapon = null;
        
        // create weapon based on type
        switch (weaponData.weaponType)
        {
            case WeaponType.Thrown:
                weapon = gameObject.AddComponent<ThrownWeapon>();
                break;
           /* case WeaponType.Shot:
                weapon = gameObject.AddComponent<ShootWeapon>();
                break;
            case WeaponType.Charged:
                weapon = gameObject.AddComponent<ChargedWeapon>();
                break;
            */
        }
        
        if (weapon != null)
        {
            weapon.weaponData = weaponData;
        }
        
        return weapon;
    }
    
    public void UnlockWeapon(WeaponDataSO weaponData)
    {
        if (!availableWeapons.Contains(weaponData))
        {
            availableWeapons.Add(weaponData);
            WeaponBase weaponInstance = CreateWeaponInstance(weaponData);
            if (weaponInstance != null)
            {
                weaponInstances[weaponData] = weaponInstance;
                weaponInstance.OnUnequip();
            }
        }
    }
}
