using UnityEngine;

public class MoonMovement : MonoBehaviour
{
    public Transform planet; // The planet or object the moon orbits around
    public float orbitDistance = 1.0f; // Distance from the planet (1 Earth radius)
    public float orbitSpeed = 2.0f; // Speed of the moon's orbit (degrees per second)
    public float orbitAngle = 5.0f; // The inclination of the orbit relative to the y-axis (in degrees)
    public float rotationSpeed = 10.0f; // Rotation speed of the moon around its axis (degrees per second)
    public float startingAngle = 0.0f; // The initial angle of the moon's orbit (in degrees), set by the user

    private float currentAngle;

    void Start()
    {
        // Set the initial angle of the moon's orbit using the startingAngle input
        currentAngle = startingAngle;

        // Optional: Log the initial angle for debugging
        Debug.Log("Initial Starting Angle: " + startingAngle);
    }

    void Update()
    {
        // Calculate the moon's new angle in orbit (using orbitSpeed)
        currentAngle += orbitSpeed * Time.deltaTime;

        // Ensure the angle stays within 0 to 360 degrees
        if (currentAngle >= 360f)
            currentAngle -= 360f;

        // Convert the angle to radians
        float angleRad = currentAngle * Mathf.Deg2Rad;

        // Apply the orbit angle (tilt) in spherical coordinates
        // Calculate the x and z position in a circular orbit, adjusted for the tilt
        float x = planet.position.x + orbitDistance * Mathf.Cos(angleRad);
        float z = planet.position.z + orbitDistance * Mathf.Sin(angleRad);

        // Adjust the y position based on the orbitAngle (tilt)
        float y = planet.position.y + orbitDistance * Mathf.Sin(orbitAngle * Mathf.Deg2Rad);

        // Set the moon's position (with tilt)
        transform.position = new Vector3(x, y, z);

        // Rotate the moon around its own axis based on the specified rotation speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
