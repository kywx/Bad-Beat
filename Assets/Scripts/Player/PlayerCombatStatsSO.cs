using UnityEngine;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerCombatStatsSO : ScriptableObject{

    public int MaxHealth = 3;
    public int AttackDamage = 1;
    public float iframes = 0.1f;
    public int KnockbackForce = 2;
    
}