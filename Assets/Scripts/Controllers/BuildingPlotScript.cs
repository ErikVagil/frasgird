using System;
using UnityEngine;

/// <summary>
/// Represents the BuildingPlot gameobjects created by
/// ColonyMapController. Owns the gameobject for the building.
/// Has a reference to one of Colony.Instance's BuildingPlots.
/// </summary>
public class BuildingPlotScript : MonoBehaviour {
  private BuildingPlot buildingPlot;
  private bool hover;
  /// <summary>
  /// Is this buildingplot being hovered over?
  /// </summary>
  public bool Hover {
    get {
      return hover;
    }
    set {
      hover = value;
      ChangeColor();
    }
  }
  private bool select;
  /// <summary>
  /// is this building plot currently selected?
  /// </summary>
  public bool Select {
    get {
      return select;
    }
    set {
      select = value;
      ChangeColor();
    }
  }

  /// <summary>
  /// The BuildingPlot object referenced by the GameObject
  /// </summary>
  public BuildingPlot BuildingPlot { get => buildingPlot; set {
    if (buildingPlot != null) {
      buildingPlot.UnsubscribeToOnBuild(OnBuild);
    }
    buildingPlot = value;
    buildingPlot.SubscribeToOnBuild(OnBuild);
    if (buildingPlot != null && buildingPlot.Building != null) {
      OnBuild(buildingPlot.Building);
    }
  } }

  /// <summary>
  /// This is the currently constructed (if not null) building prefab
  /// representing the building on this plot.
  /// </summary>
  private GameObject buildingObject {get; set;}
  void OnEnable()
  {
    CleanupManager.Instance.SubscribeToCleanup(Cleanup);
  }

  /// <summary>
  /// Handle color change logic based on hovering and selecting.
  /// </summary>
  private void ChangeColor() {
    Color color;
      if (hover) {
        color = select ? Color.cyan : Color.blue;
      } else {
        color = select ? Color.green : Color.white;
      }
      gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
  }

  /// <summary>
  /// Called by the BuildingPlot to notify the game object
  /// to change the building prefab displayed.
  /// </summary>
  /// <param name="building"></param>
  private void OnBuild(Building building) {
    if (buildingObject != null) {
      Destroy(buildingObject);
      buildingObject = null;
    }

    if (building == Buildings.Empty) {
      return;
    }

    GameObject prefab;
    if (building == Buildings.Generator) {
      prefab = ModelController.Instance.PowerPrefab;
    } else if (building == Buildings.HydroRecycler) {
      prefab = ModelController.Instance.HydroPrefab;
    } else if (building == Buildings.Farm) {
      prefab = ModelController.Instance.FarmPrefab;
    } else {
      prefab = ModelController.Instance.HousingPrefab;
    }
    
    GameObject go = Instantiate(prefab);
    go.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    go.transform.position = new Vector3(gameObject.transform.position.x, go.transform.localScale.y / 2, gameObject.transform.position.z);
    // go.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
    buildingObject = go;
    
  }

  /// <summary>
  /// unlink from the Colony when this script gets destroyed
  /// </summary>
  private void Cleanup() {
    if (buildingPlot != null) {
      buildingPlot.UnsubscribeToOnBuild(OnBuild);
    }
  }
}