using UnityEngine;

public interface IWeapon
{
    void Attack(Vector2 direction, Transform attackPoint);
    bool CanAttack();
    void OnEquip();
    void OnUnequip();
}
