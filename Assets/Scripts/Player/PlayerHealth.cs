using UnityEngine;

public class PlayerHealth : MonoBehaviour{

    private int _health;

    public PlayerCombatStatsSO stats;
    //
    private PlayerRespawn RespawnManager;

    private float iframeTimer;

    
    private void Awake(){
        RespawnManager = this.GetComponent<PlayerRespawn>();
        _health = stats.MaxHealth;
        iframeTimer = 0;
    }

    private void Update()
    {
        if(iframeTimer > 0)
        {
            iframeTimer -= Time.deltaTime;
        }
    }
    
    public void Damage(int dmg){
        if(iframeTimer <= 0){
            _health -= dmg;
            if(_health <= 0 && RespawnManager != null){
                RespawnManager.Respawn();
            }
        }
    }

    public void Heal(int amt){
        _health = Mathf.Max(stats.MaxHealth, _health + amt);
    }

    public void HealToMax(){
        _health = stats.MaxHealth;
    }

}