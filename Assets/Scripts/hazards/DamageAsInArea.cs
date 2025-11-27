using System.Collections;
using UnityEngine;

public class DamageAsInArea : MonoBehaviour {
    public int damage_value;
    private bool debounce = false;

    IEnumerator Debounce(float delay) {
        debounce = true;
        yield return new WaitForSeconds(delay);
        debounce = false;
    }

    void OnCollisionStay2D(Collision2D collision) {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if (health != null && !debounce) {
            StartCoroutine(Debounce(0.5f));
            health.Damage(damage_value);
        }
    }
}
