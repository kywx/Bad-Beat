using UnityEngine;

public class ObstacleCheck : MonoBehaviour
{
    public bool obstacleDetected = false;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            obstacleDetected = true;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            obstacleDetected = false;
        }
    }
}
