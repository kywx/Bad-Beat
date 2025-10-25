using UnityEngine;

public class PlayerHealth : MonoBehaviour{

    private int _health;

    public PlayerCombatStatsSO stats;
    //
    private PlayerRespawn RespawnManager;

    
    private void Awake(){
        RespawnManager = this.GetComponent<PlayerRespawn>();
        _health = stats.MaxHealth;
    }
    
    public void Damage(int dmg){
        _health -= dmg;
        if(_health <= 0 && RespawnManager != null){
            RespawnManager.Respawn();
        }
    }

    public void Heal(int amt){
        _health = Mathf.Max(stats.MaxHealth, _health + amt);
    }

    public void HealToMax(){
        _health = stats.MaxHealth;
    }

}