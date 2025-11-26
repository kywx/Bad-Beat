using System;
using UnityEngine;
using System.Threading.Tasks;

public class BossMovement : MonoBehaviour
{
    public GameObject targetA; // First target
    public GameObject targetB; // Second target

    public float moveSpeed; // Units per second
    public float delay; // Time in seconds it will wait once reatching a target before moving on

    private Transform curTarget; // Current target
    private bool delayed; // Determines if it should move or not

    void Start() {
        curTarget = targetA.transform;
        delayed = false;
        targetA.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f); // Turns target blocks invisible
        targetB.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    async Task Update() {
        if (!delayed) {
            transform.position = Vector3.MoveTowards(transform.position, curTarget.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, curTarget.position) < 0.01f) {
                delayed = true;
                curTarget = (curTarget == targetA.transform ? targetB.transform : targetA.transform);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                delayed = false;
            }
        }
    }
}