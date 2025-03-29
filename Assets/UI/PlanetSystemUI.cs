using UnityEngine;
using UnityEngine.UIElements;

public class PlanetSystemUI : MonoBehaviour
{

    private  GameObject currentPlanetSystem; // holds the associated planet
    

    public void SetPlanet(GameObject planetSystem)
    {
        this.currentPlanetSystem = planetSystem;
    }

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Label planetName = root.Q<Label>("planetName");
        planetName.text = currentPlanetSystem.name;
        Label planetInfo = root.Q<Label>("planetInfo");
        planetInfo.text = currentPlanetSystem.GetComponent<PlanetSystem>().GetDescription();
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
