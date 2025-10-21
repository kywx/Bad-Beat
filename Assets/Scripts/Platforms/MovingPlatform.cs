using System;
using System.Threading.Tasks;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public Transform targetA; // First target
    public Transform targetB; // Second target
    public float moveSpeed; // Units per second
    public float rotateSpeed; // Degrees per second
    public float delay; // Time in seconds it will wait once reatching a target before moving on
    public bool requireRotation; // Requires the platform finishes rotating before moving to next target
    public bool infRotate; // Determines if rotations should be done infinitely (For something like a saw blade))
    public bool rotateRight; // When indefinite, if true will rotate right, if false will rotate left

    private Transform curTarget; // Current target
    private bool delayed; // Determines if it should move or not
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        curTarget = targetA;
        delayed = false;
    }

    // Update is called once per frame
    async Task Update() {
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
                curTarget = (curTarget == targetA ? targetB : targetA);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                delayed = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(null);
        }
    }
}