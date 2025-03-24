using UnityEngine;
using UnityEngine.UIElements;

public class PlanetUI : MonoBehaviour
{

    private  GameObject currentPlanet; // holds the associated planet
    

    public void SetPlanet(GameObject planet)
    {
        currentPlanet = planet;
    }

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Label planetName = root.Q<Label>("planetName");
        planetName.text = currentPlanet.name;
        Label status = root.Q<Label>("planetInfo");
        Button sendButton = root.Q<Button>("sendButton");
        Button backButton = root.Q<Button>("backButton");

        backButton.clicked += () =>
        {
            CameraController cameraController = Object.FindFirstObjectByType<CameraController>();
            if(cameraController != null)
            {
                cameraController.ReturnToGalaxyView();
            }
            else
            {
                Debug.LogError("CameraController not found");
            }
        };
    }

    private void getPlayerResources()
    {
        
    }
}
