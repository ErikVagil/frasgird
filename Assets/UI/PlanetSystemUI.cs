using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class PlanetSystemUI : MonoBehaviour
{

    private  GameObject currentPlanetSystem; // holds the associated planet
    private CameraController cameraController; // holds the camera controller
    
    private float rotationSpeed = 4f; // Speed for orbit rotation

    // UI elements
    private Label planetName;
    private Label planetInfo;
    private Button sendButton;
    private Button backButton;
    private VisualElement expeditionSupplyUI;
    private VisualElement root;
    private VisualElement playerResourcesUI;

    public void SetPlanet(GameObject planetSystem)
    {
        this.currentPlanetSystem = planetSystem;
    }

    public void SetCameraController(CameraController cameraController)
    {
        this.cameraController = cameraController;
    }

    private void setUIElements(VisualElement root)
    {
        this.root = root;
        planetName = root.Q<Label>("planetName");
        planetInfo = root.Q<Label>("planetInfo");
        sendButton = root.Q<Button>("sendButton");
        backButton = root.Q<Button>("backButton");
        expeditionSupplyUI = root.Q<VisualElement>("expeditionSupply");
        playerResourcesUI = root.Q<VisualElement>("playerResources");
    }

    private void OnEnable()
    {
        // Get the root VisualElement and set the UI elements
        root = GetComponent<UIDocument>().rootVisualElement;
        setUIElements(root);

        // Update player resources UI
        playerResourcesUI.Q<Label>("playerWater").text = "Water: " + Colony.Instance.GetStockpile(Goods.Water).ToString();
        playerResourcesUI.Q<Label>("playerFood").text = "Food: " + Colony.Instance.GetStockpile(Goods.Food).ToString();
        playerResourcesUI.Q<Label>("playerPower").text = "Power: " + Colony.Instance.PowerSurplus.ToString();
        playerResourcesUI.Q<Label>("playerPopulation").text = "Population: " + Colony.Instance.Population.ToString();
        // Set planet name and info
        planetName.text = currentPlanetSystem.name;
        planetInfo.text = currentPlanetSystem.GetComponent<PlanetSystem>().GetDescription();

        backButton.clicked += () =>
        {
            if(cameraController != null)
            {
                cameraController.ReturnToGalaxyView();
            }
            else
            {
                Debug.LogError("CameraController not found");
            }
        };

        sendButton.clicked += () =>
        {
            if (currentPlanetSystem != null)
            {
                Debug.Log("Send expedition to " + currentPlanetSystem.name);
                planExpedition();
            }
            else
            {
                Debug.LogError("Current planet system not set");
            }
        };
    }

    IEnumerator RotateCamera()
        {
            float totalRotation = 0f;
            while (totalRotation < 360f)
            {
                float rotationThisFrame = rotationSpeed * Time.deltaTime;
                cameraController.transform.RotateAround(currentPlanetSystem.transform.position, Vector3.up, rotationThisFrame);
                totalRotation += rotationThisFrame;
                yield return null;
            }
            Debug.Log("Camera rotation around the planet complete.");
        }

    private void planExpedition()
    {
        if (cameraController != null && currentPlanetSystem != null)
        {
            StartCoroutine(RotateCamera());
            // Change the visibility of the expedition supply UI
            expeditionSupplyUI.style.display = DisplayStyle.Flex;

            // Set the text of the send button to "Launch"
            sendButton.text = "Launch";
        }
        else
        {
            Debug.LogError("Missing cameraController or currentPlanetSystem");
        }
    }
}
