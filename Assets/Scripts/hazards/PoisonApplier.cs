using UnityEngine;

// a script to be put on objects (hazards, projectiles) that poison the player
public class PoisonApplier : MonoBehaviour
{
    public int damagePerTick;
    public float timeBetweenTicks;
    public float duration;

    void OnTriggerEnter2D(Collider2D collider){
        PlayerPoisoner poisoner = collider.gameObject.GetComponentInParent<PlayerPoisoner>();

        if(poisoner != null){
            poisoner.Poison(duration, damagePerTick, timeBetweenTicks);
        }
    }
}