using Unity.VisualScripting;
using UnityEngine;
public class MouseController : MonoBehaviour {
  private BuildingPlotScript hover = null;
  public static MouseController Instance { get; private set; }
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    }
  }
  void Update() {
    // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition) + " " + Input.mousePosition);
    if (hover != null) {
      hover.Unhover();
      hover = null;
    }
    RaycastHit raycastHit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out raycastHit, 300f) && raycastHit.collider != null) {
      BuildingPlotScript plot = raycastHit.collider.gameObject.GetComponent<BuildingPlotScript>();
      if (plot != null) {
        hover = plot;
        hover.Hover();
      }
    }
    if (Input.GetMouseButtonDown(0) && hover != null) {
      hover.BuildingPlot.Build(Buildings.Generator);
    }

  }
}