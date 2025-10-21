using System;
using System.Threading.Tasks;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;
    public float moveSpeed;
    public float rotateSpeed;
    public float delay;

    private Transform curTarget;
    private bool delayed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        curTarget = targetA;
        delayed = false;
    }

    // Update is called once per frame
    async Task Update() {
        if (!delayed) {
            transform.position = Vector3.MoveTowards(transform.position, curTarget.position, speed * Time.deltaTime);
            transform.rotation = Vector3.RotateTowards(transform.rotation, curTarget.rotation, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, curTarget.position) < 0.1f) {
                delayed = true;
                curTarget = (curTarget == targetA ? targetB : targetA);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                delayed = false;
            }
        }
    }
}