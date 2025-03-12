using UnityEngine;

public class CloudRotation : MonoBehaviour
{
    public float cloudRotationSpeed = 0.6f;  // Approximate average speed of clouds relative to Earth's rotation (1/10th speed)

    void Update()
    {
        // Rotate the cloud layer around the planet's Y-axis at a slower speed
        transform.Rotate(Vector3.up * cloudRotationSpeed * Time.deltaTime);
    }
}
