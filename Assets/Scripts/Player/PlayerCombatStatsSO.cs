using UnityEngine;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerCombatStatsSO : ScriptableObject{

    public int MaxHealth = 3;
    public int AttackDamage = 1;
    public float iframes = 1;
    public int KnockbackForce = 2;
    
}