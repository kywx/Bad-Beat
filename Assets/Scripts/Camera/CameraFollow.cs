using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _damping;
    public Transform target; // target to follow

    private Vector3 _velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + _offset;
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _damping);
    }
    void Update()
    {
        
    }
}
