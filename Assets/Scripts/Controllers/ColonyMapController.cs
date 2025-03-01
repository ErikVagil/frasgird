using UnityEngine;
public class ColonyMapController : MonoBehaviour {
  public GameObject BuildingPlotPrefab;
  public static ColonyMapController Instance { get; private set; }
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    }
  }
  void Start() {
    GameObject go = Instantiate(BuildingPlotPrefab);
    go.transform.position = new Vector3(0, 0, 0);
    go.GetComponent<BuildingPlotScript>().BuildingPlot = Colony.Instance.Map.GetPlot(0, 0);
  }
}