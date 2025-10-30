using System;
using UnityEngine;

public class SyncedRotation : MonoBehaviour
{
    [SerializeField] private int _rotationsPerLoop;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // takes one entire loop of a song for the rotation to be complete
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360*_rotationsPerLoop, Conductor.instance.loopPositionInAnalog));
    }
}
