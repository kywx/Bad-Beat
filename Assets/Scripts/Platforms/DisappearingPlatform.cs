using System;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DisappearingPlatform : MonoBehaviour
{
    public float delay;
    public float changeDuration;
    public bool fadeEnabled;

    private bool visible;
    private Color curCol;

    private void Start() {
        visible = true;
        curCol = this.GetComponent<SpriteRenderer>().color;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter2D(Collision2D collision) {
        if (fadeEnabled & visible & collision.gameObject.CompareTag("Player")) {
            Invoke("fade", delay);
        }
    }

    private void fade() {
        if (visible) {
            curCol.a = 0f; // Turns platform invisible
            visible = false;
            this.GetComponent<SpriteRenderer>().color = curCol;
            this.GetComponent<Rigidbody2D>().simulated = false;
            Invoke("fade", changeDuration);
        } else {
            curCol.a = 1f; // Turns platform visible
            this.GetComponent<Rigidbody2D>().simulated = true;
            this.GetComponent<SpriteRenderer>().color = curCol;
            visible = true;
        }
    }
}