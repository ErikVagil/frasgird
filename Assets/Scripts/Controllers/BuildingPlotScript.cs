using UnityEngine;

public class BuildingPlotScript : MonoBehaviour {
  private BuildingPlot buildingPlot;
  public BuildingPlot BuildingPlot { get => buildingPlot; set {
    buildingPlot = value;
    buildingPlot.SubscribeToOnBuild(OnBuild);
  } }
  private GameObject buildingObject {get; set;}
  public void Hover() {
    this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
  }
  public void Unhover() {
    this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
  }
  private void OnBuild(Building building) {
    if (buildingObject != null) {
      Debug.Log("plot not empty");
      return;
    }
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