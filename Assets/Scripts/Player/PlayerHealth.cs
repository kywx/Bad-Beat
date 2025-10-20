using UnityEngine;

public class PlayerHealth : MonoBehaviour{

    private int _health;
    public int MaxHealth;
    private PlayerRespawn RespawnManager;

    
    private void Awake(){
        RespawnManager = this.GetComponent<PlayerRespawn>();
        _health = MaxHealth;
    }
    
    public void Damage(int dmg){
        _health -= dmg;
        if(_health <= 0 && RespawnManager != null){
            RespawnManager.Respawn();
        }
    }

    public void Heal(int amt){
        _health = Mathf.Max(MaxHealth, _health + amt);
    }

    public void HealToMax(){
        _health = MaxHealth;
    }

}