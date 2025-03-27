using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 6f; // Default rotation speed: 6 degrees per second (1 full rotation per minute)

    void Update()
    {
        // Rotate the planet around its Y-axis based on the rotation speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
