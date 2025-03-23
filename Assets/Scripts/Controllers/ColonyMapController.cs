using UnityEngine;
public class ColonyMapController : MonoBehaviour {
  public static ColonyMapController Instance { get; private set; }
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    }
  }
  void Start() {
    // CreatePlot(Colony.Instance.Map.GetPlot(0, 0), 0, 0, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(0, 1), 0, 10, false);

    // CreatePlot(Colony.Instance.Map.GetPlot(1, 0), 10, -10, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(1, 1), -10, -10, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(1, 2), -15, 5, true);
    // CreatePlot(Colony.Instance.Map.GetPlot(1, 3), 10, 20, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(1, 4), -10, 20, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(1, 5), 15, 5, true);
    
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 0), 20, -20, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 1), 0, -20, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 2), -20, -20, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 3), -25, -5, true);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 4), -25, 15, true);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 5), -20, 30, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 6), 0, 30, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 7), 20, 30, false);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 8), 25, -5, true);
    // CreatePlot(Colony.Instance.Map.GetPlot(2, 9), 25, 15, true);
    for (int i = 0; i < Colony.Instance.Map.NumRings; i++) {
      int id = 0;
      for (int j = 0; j < 1 + i; j++) {
        CreatePlot(Colony.Instance.Map.GetPlot(i, id), 10 * i - 20 * j, -10 * i, false);
        id++;
      }
      for (int j = 0; j < i; j++) {
        CreatePlot(Colony.Instance.Map.GetPlot(i, id), -5 + -10 * i, 15 - 10 * i + 20 * j, true);
        id++;
      }
      for (int j = 0; j < 1 + i; j++) {
        CreatePlot(Colony.Instance.Map.GetPlot(i, id), -10 * i + 20 * j, 10 + 10 * i, false);
        id++;
      }
      for (int j = 0; j < i; j++) {
        CreatePlot(Colony.Instance.Map.GetPlot(i, id), 5 + 10 * i, -5 + 10 * i - 20 * j, true);
        id++;
      }
    }
  }
  private void CreatePlot(BuildingPlot plot, int x, int z, bool rotate) {
    GameObject go = Instantiate(ModelController.Instance.BuildingPlotPrefab);
    go.transform.position = new Vector3(x, 0, z);
    if (rotate) {
      go.transform.Rotate(0, 90, 0);
    }
    go.GetComponent<BuildingPlotScript>().BuildingPlot = plot;
  }
}