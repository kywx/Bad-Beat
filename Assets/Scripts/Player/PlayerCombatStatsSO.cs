using UnityEngine;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerCombatStatsSO : ScriptableObject{

    private int _health;
    public int MaxHealth = 2;
    public int AttackDamage = 1;
    
}