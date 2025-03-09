using UnityEngine;

public class BuildingPlotScript : MonoBehaviour {
  private BuildingPlot buildingPlot;
  private bool hover;
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
  public bool Select {
    get {
      return select;
    }
    set {
      select = value;
      ChangeColor();
    }
  }

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
  private GameObject buildingObject {get; set;}
  void OnEnable()
  {
    CleanupManager.Instance.SubscribeToCleanup(Cleanup);
  }
  private void ChangeColor() {
    Color color;
      if (hover) {
        color = select ? Color.cyan : Color.blue;
      } else {
        color = select ? Color.green : Color.white;
      }
      gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
  }
  private void OnBuild(Building building) {
    Debug.Log("onbuild");
    if (buildingObject != null && building != null) {
      Debug.Log("already has go");
      return;
    } else if (building == null) {
      Destroy(buildingObject);
      buildingObject = null;
    } else {
      Color color = Color.white;
      if (building == Buildings.Generator) {
        color = Color.yellow;
      } else if (building == Buildings.HydroRecycler) {
        color = Color.cyan;
      } else if (building == Buildings.Farm) {
        color = Color.green;
      }
      GameObject go = Instantiate(ColonyMapController.Instance.BuildingPrefab);
      go.transform.position = new Vector3(gameObject.transform.position.x, go.transform.localScale.y / 2, gameObject.transform.position.z);
      go.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
      buildingObject = go;
    }
  }
  private void Cleanup() {
    if (buildingPlot != null) {
      buildingPlot.UnsubscribeToOnBuild(OnBuild);
    }
  }
}