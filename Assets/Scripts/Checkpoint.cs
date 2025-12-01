using UnityEngine;

public class Checkpoint : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider){
        PlayerRespawn player = collider.GetComponentInParent<PlayerRespawn>();

        if(player != null){
            Vector2 new_spawn = this.transform.position;

            player.UpdateCheckpoint(new_spawn, this.transform.position.x);
            player.GetComponent<PlayerHealth>().HealToMax();
        }
    }
}