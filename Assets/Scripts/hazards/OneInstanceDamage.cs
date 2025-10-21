using UnityEngine;

public class OneInstanceDamage : MonoBehaviour {
    public int damage_value;

    void OnCollisionEnter2D(Collision2D collider){
        PlayerHealth health = collider.gameObject.GetComponent<PlayerHealth>();

        if(health != null){
            health.Damage(damage_value);
        }
    }
}