using UnityEngine;

public class Checkpoint : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider){
        PlayerHealthAndRespawn player = collider.GetComponentInParent<PlayerHealthAndRespawn>();

        if(player != null){
            Vector2 new_spawn = this.transform.position;

            player.UpdateCheckpoint(new_spawn);
        }
    }
}