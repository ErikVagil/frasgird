using UnityEngine;

/// <summary>
/// A controller for referencing prefabs from the scripts. There's
/// probably a better way to do this? But it was quite simple to set up.
/// </summary>
public class ModelController : MonoBehaviour {
  public static ModelController Instance { get; private set; }
  public GameObject BuildingPrefab;
  public GameObject BuildingPlotPrefab;
  public GameObject FarmPrefab;
  public GameObject HydroPrefab;
  public GameObject PowerPrefab;
  public GameObject HousingPrefab;
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    } // else error?
  }
}