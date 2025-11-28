using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    public bool playerDetected = false;
    public Vector2 playerPosition;
    protected void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerHealth player = collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            playerDetected = true;
            playerPosition = collision.transform.position;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            playerDetected = false;
            playerPosition = collision.transform.position;

        }
    }

}
