using UnityEngine;

public class MoonMovement : MonoBehaviour
{
    public Transform planet; // The planet the moon orbits
    public float orbitSpeed = 0.2f; // Speed of the moon's orbit (slower than Earth's rotation)
    public float orbitAngle = 0f; // Angle at which the orbit rotates around the planet (in degrees)
    public float rotationSpeed = 1f; // Speed of the moon's own rotation (this is quite slow)
    public float rotationAngle = 0f; // Starting angle of the moon's rotation (in degrees)

    void Start()
    {
        // Apply the initial rotation angle to the moon's starting position
        transform.Rotate(Vector3.up, rotationAngle);
    }

    void Update()
    {
        if (planet != null)
        {
            // Rotate the moon around the planet using its position with an adjustable orbit angle
            transform.RotateAround(planet.position, Quaternion.Euler(0, orbitAngle, 0) * Vector3.up, orbitSpeed * Time.deltaTime);
            
            // Rotate the moon around its own axis to simulate its rotation
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
