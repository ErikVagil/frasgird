using UnityEngine;

public class BuildingPlotScript : MonoBehaviour {
  public BuildingPlot BuildingPlot { get; set; }
  public void Hover() {
    this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
  }
  public void Unhover() {
    this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
  }
}