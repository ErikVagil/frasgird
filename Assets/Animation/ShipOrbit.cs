using UnityEngine;

public class ShipOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform orbitCenter; // The point to orbit around
    private const float ORBIT_DISTANCE = 0.75f; // Fixed distance from the center
    public float orbitSpeed = 20f; // Degrees per second
    public float orbitTilt = 0f; // Adjustable tilt in degrees

    private float currentAngle = 0f;
    private Vector3 previousPosition;

    void Start()
    {
        // Set initial position
        if (orbitCenter != null)
        {
            Vector3 offset = GetTiltedOffset(0);
            transform.position = orbitCenter.position + offset;
            previousPosition = transform.position;
        }
    }

    void Update()
    {
        if (orbitCenter == null) return;

        // Update angle based on orbit speed
        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle %= 360f;

        // Calculate new position with tilt
        Vector3 offset = GetTiltedOffset(currentAngle);
        transform.position = orbitCenter.position + offset;

        // Orient towards direction of movement
        Vector3 movementDirection = (transform.position - previousPosition).normalized;
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            targetRotation *= Quaternion.Euler(0, 90, -90);
            transform.rotation = targetRotation;
        }

        previousPosition = transform.position;
    }

    private Vector3 GetTiltedOffset(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Quaternion tiltRotation = Quaternion.Euler(orbitTilt, 0, 0); // Apply tilt to orbit plane

        // Calculate position in local space, then rotate by tilt
        Vector3 localOffset = new Vector3(Mathf.Cos(radians) * ORBIT_DISTANCE, 0, Mathf.Sin(radians) * ORBIT_DISTANCE);
        return tiltRotation * localOffset;
    }
}
