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
  // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
  RaycastHit raycastHit;
  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
  if (Physics.Raycast(ray, out raycastHit, 100f) && raycastHit.collider != null) {
    BuildingPlotScript plot = raycastHit.collider.gameObject.GetComponent<BuildingPlotScript>();
    if (hover != null) {
      hover.Unhover();
    }
    hover = plot;
    hover.Hover();
  } else {
    if (hover != null) {
      hover.Unhover();
      hover = null;
    }
  }

  }
}