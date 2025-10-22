using System;
using System.Threading.Tasks;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float delay = 1f;
    public float respawnTime = 2f;

    private bool visible = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Task.Delay(TimeSpan.FromSeconds(delay));

            Task.Delay(TimeSpan.FromSeconds(respawnTime));

        }
    }

    private void fade() {
        float timer = 0f;

    }
}
