using System;
using System.Threading.Tasks;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public GameObject targetA; // First target
    public GameObject targetB; // Second target

    public float moveSpeed; // Units per second
    public float rotateSpeed; // Degrees per second
    public float delay; // Time in seconds it will wait once reatching a target before moving on
    public bool requireRotation; // Requires the platform finishes rotating before moving to next target
    public bool infRotate; // Determines if rotations should be done infinitely (For something like a saw blade))
    public bool rotateRight; // When indefinite, if true will rotate right, if false will rotate left
    public bool waitForPlayer;

    private Transform curTarget; // Current target
    private bool delayed; // Determines if it should move or not
    
    void Start() {
        curTarget = targetA.transform;
        delayed = false;
        targetA.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f); // Turns target blocks invisible
        targetB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    async Task Update() {
        if (waitForPlayer) {
            return;
        }
        if (infRotate) {
            if (rotateRight) {
                transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime * -1);
            } else {
                transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime);
            }
        }
        if (!delayed) {
            transform.position = Vector3.MoveTowards(transform.position, curTarget.position, moveSpeed * Time.deltaTime);
            if (!infRotate) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, curTarget.rotation, rotateSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(transform.position, curTarget.position) < 0.01f) {
                if (requireRotation & Quaternion.Angle(transform.rotation, curTarget.rotation) > 1) {
                    return;
                }
                delayed = true;
                curTarget = (curTarget == targetA.transform ? targetB.transform : targetA.transform);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                delayed = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            waitForPlayer = false;
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(null);
        }
    }
}