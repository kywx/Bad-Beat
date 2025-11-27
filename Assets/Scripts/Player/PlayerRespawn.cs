using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour {
    public Vector2 default_spawn_point; //default spawn point to be set for each scene.
    private PlayerHealth HealthManager;
    public Image BlackImage;
    public float FadeDuration = 0.5f;
    private PlayerPoisoner PoisonManager;

    private Vector2 spawn_point;

    public void UpdateCheckpoint(Vector2 new_pos){
        spawn_point = new_pos;
    }

    public void Awake(){
        HealthManager = this.GetComponent<PlayerHealth>();
        PoisonManager = this.GetComponent<PlayerPoisoner>();
    }

    public void Respawn() {
        this.transform.position = spawn_point;
        BlackImage.color = Color.black;
        if (HealthManager != null)
        {
            //HealthManager.HealToMax();
        }
        if (PoisonManager != null)
        {
            PoisonManager.CurePoison();
        }
        StartCoroutine(FadeCoroutine());
    }

    public void Warp() {
        this.transform.position = spawn_point;
        BlackImage.color = Color.black;
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine() {
        float timer = 0f;
        Color curColor = BlackImage.color;

        while (timer < FadeDuration) {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1, 0, timer / FadeDuration);
            BlackImage.color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
            yield return null;
        }
    }
}