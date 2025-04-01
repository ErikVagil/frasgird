using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform targetPlanet;
    private bool isFocusing = false;

    public GameObject planetUI;
    
    

    public float zoomSpeed = 2f; // Adjust for smoothness
    public float orbitDistance = 5f; // Distance from the planet

    // Right now the way I have the background won't work with the camera rotating around the planet but
    // Could be a fun idea if we change the way the background works
    // public float rotationSpeed = 2f; // Speed for orbit rotation

    void Start()
    {
        // Save the original positions to save the overview position
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        planetUI.SetActive(false);
    }

    void Update()
    {
        // Focus on the planet that is clicked
        if (Input.GetMouseButtonDown(0) && !isFocusing) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Planet"))
                {
                    FocusOnPlanet(hit.collider);
                }
            }
        }

        // Return to galaxy view if user presses Esc
        // if (Input.GetKeyDown(KeyCode.Escape) && isFocusing)
        // {
        //     ReturnToGalaxyView();
        // }

        // if (isFocusing && targetPlanet != null)
        // {
        //     // Keep orbiting around the planet
        //     // transform.RotateAround(targetPlanet.position, Vector3.up, rotationSpeed * Time.deltaTime);
        //     // transform.LookAt(targetPlanet);
        // }
    }

    void FocusOnPlanet(Collider collider)
    {   
        Transform planet = collider.transform;
        targetPlanet = planet;
        isFocusing = true;

        // Set the clicked planet in the PlanetUI
        planetUI.GetComponent<PlanetSystemUI>().SetPlanet(collider.gameObject);

        // Set the camera controller in the PlanetUI
        planetUI.GetComponent<PlanetSystemUI>().SetCameraController(this);

        // Set the UI active
        planetUI.SetActive(true);

        // Calculate new camera position (orbit around the planet)
        Vector3 targetPosition = planet.position + planet.forward * -orbitDistance;

        // Smooth transition to new position
        StartCoroutine(MoveCamera(targetPosition, Quaternion.LookRotation(planet.position - targetPosition)));
    }

    public void ReturnToGalaxyView()
    {
        isFocusing = false;
        targetPlanet = null;
        planetUI.SetActive(false);

        // Smooth transition back to original position
        StartCoroutine(MoveCamera(originalPosition, originalRotation));
    }

    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            yield return null;
        }
    }
}
