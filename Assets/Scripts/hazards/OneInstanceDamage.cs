using UnityEngine;

public class OneInstanceDamage : MonoBehaviour {
    public int damage_value;

    void OnCollisionEnter2D(Collision2D collider){
        PlayerCombatStats health = collider.gameObject.GetComponent<PlayerCombatStats>();

        if(health != null){
            health.Damage(damage_value);
        }
    }
}