using UnityEngine;

public class OneInstanceDamage : MonoBehaviour {
    public int damage_value;

    void OnCollisionEnter2D(Collision2D collider){
        PlayerHealthAndRespawn health = collider.gameObject.GetComponent<PlayerHealthAndRespawn>();

        if(health != null){
            health.Damage(damage_value);
        }
    }
}