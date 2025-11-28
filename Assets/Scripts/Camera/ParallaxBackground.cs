using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float startpos;
    public GameObject Camera;
    public float ParallaxEffect;
    void Start() {
        startpos = transform.position.x;
    }

    void FixedUpdate() {
        float dist = (Camera.transform.position.x - startpos) * ParallaxEffect;

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}