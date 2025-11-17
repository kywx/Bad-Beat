using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon System")]
    public List<WeaponDataSO> availableWeapons = new List<WeaponDataSO>();
    public Transform attackPoint;
    public SpriteRenderer handSpriteRenderer;
    public PlayerMovement playerMovement; // assign via Inspector or GetComponent

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
        // continuously update weapon image and position based on facing direction
        if (currentWeapon != null)
    {
        UpdateHandVisual(availableWeapons[currentWeaponIndex]);

    }
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
    
public Vector2 GetAttackDirection()
{  bool isFacingRight = playerMovement.IsFacingRight;
   float baseAngle = isFacingRight ? 8f : 172f; // 0° for right, 180° for left

    float fireAngleRad = baseAngle * Mathf.Deg2Rad;
    Vector2 direction = new Vector2(Mathf.Cos(fireAngleRad), Mathf.Sin(fireAngleRad));
    return direction.normalized;

}
    
    void HandleWeaponSwitching()
    {

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
           
        }
    }

    

            // Called when you switch weapons
    public void UpdateHandVisual(WeaponDataSO weaponData)
        {
            if (handSpriteRenderer == null) return;

                handSpriteRenderer.sprite = weaponData.weaponIcon;
                Transform handTransform = handSpriteRenderer.transform;

                bool isFacingRight = playerMovement.IsFacingRight;

                // Example position offset values: adjust to suit your game visuals
                Vector3 rightPosOffset = new Vector3(0.5f, 0f, 0f);
                Vector3 leftPosOffset = new Vector3(-0.5f, 0f, 0f);

                // Position the weapon relative to attackPoint
                handTransform.position = attackPoint.position + (isFacingRight ? rightPosOffset : leftPosOffset);
                
                // Flip the weapon sprite horizontally by adjusting scale.x
                handTransform.localScale = new Vector3(isFacingRight ? 0.003f : -0.003f, 0.003f, 1f);
                handSpriteRenderer.flipX = !isFacingRight;
    

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
