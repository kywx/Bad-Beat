using UnityEngine;

public class PlayerRespawn : MonoBehaviour {
    public Vector2 default_spawn_point; //default spawn point to be set for each scene.
    private PlayerHealth HealthManager;
    private PlayerPoisoner PoisonManager;

    private Vector2 spawn_point;

    public void UpdateCheckpoint(Vector2 new_pos){
        spawn_point = new_pos;
    }

    public void Awake(){
        HealthManager = this.GetComponent<PlayerHealth>();
        PoisonManager = this.GetComponent<PlayerPoisoner>();
    }

    public void Respawn(){
        this.transform.position = spawn_point;
        if (HealthManager != null)
        {
            HealthManager.HealToMax();
        }
        if (PoisonManager != null)
        {
            PoisonManager.CurePoison();
        }
    }
}