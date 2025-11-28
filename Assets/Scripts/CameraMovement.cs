using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.identity;
    }
}
