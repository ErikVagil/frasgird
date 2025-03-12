using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public float rotationSpeed = 6f;  // Earth’s rotation speed (6° per second for 1 minute = 1 day)
    public float tiltAngle = 23.5f;   // Earth’s axial tilt relative to its orbital plane

    void Start()
    {
        // Set the initial tilt on the X-axis (23.5° tilt)
        transform.rotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }

    void Update()
    {
        // Rotate the planet around its Y-axis at the specified speed (6° per second)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
