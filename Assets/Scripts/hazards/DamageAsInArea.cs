using UnityEngine;

public class DamageAsInArea : MonoBehaviour {
    public int damage_value;

    void OnCollisionStay2D(Collision2D collision) {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if (health != null) {
            health.Damage(damage_value);
        }
    }
}
