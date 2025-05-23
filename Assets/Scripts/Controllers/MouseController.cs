using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Could probably be named something more descriptive. This class
/// controls the selection of BuildingPlot script objects. Access
/// its instance member to get the currently selected plot.
/// </summary>
public class MouseController : MonoBehaviour {
  private BuildingPlotScript hover = null;
  private BuildingPlotScript selected = null;

  /// <summary>
  /// The current BuildingPlot gameobject selected.
  /// MAY BE NULL
  /// </summary>
  public BuildingPlotScript SelectedPlot {
    get => selected;
  }
  public static MouseController Instance { get; private set; }
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    }
    hover = null;
    selected = null;
  }
  void Update() {
    // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition) + " " + Input.mousePosition);
    if (hover != null) {
      hover.Hover = false;
      hover = null;
    }
    RaycastHit raycastHit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out raycastHit, 300f) && raycastHit.collider != null) {
      BuildingPlotScript plot = raycastHit.collider.gameObject.GetComponent<BuildingPlotScript>();
      if (plot != null) {
        hover = plot;
        hover.Hover = true;
      }
    }
    if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null) {
      if (hover != null) {
        if (selected != null) {
          selected.Select = false;
        }
        selected = hover;
        selected.Select = true;
        ColonyUIController.Instance.ShowPlotMenu();
      } else {
        ColonyUIController.Instance.HidePlotMenu();
        if (selected != null) {
          selected.Select = false;
        }
        selected = null;
      }
    }

  }
}