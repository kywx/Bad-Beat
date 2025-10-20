using UnityEngine;

public class PlayerHealthAndRespawn : MonoBehaviour{

    public Vector2 default_spawn_point; //default spawn point to be set for each scene.

    private Vector2 spawn_point;
    private int health;
    public int max_health;
    
    
    private void Awake(){
        spawn_point = default_spawn_point;
        health = max_health;
    }
    
    public void Damage(int dmg){
        health -= dmg;
        if(health <= 0){
            Respawn();
        }
    }

    public void Update_checkpoint(Vector2 new_pos){
        spawn_point = new_pos;
    }

    private void Respawn(){
        this.transform.position = spawn_point; 
        health = max_health;
    }

}