using UnityEngine;
using TMPro;
using System.Linq;
public class ColonyUIController : MonoBehaviour {
  public static ColonyUIController Instance { get; private set; }
  public GameObject canvas;
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    } // else error?

    UpdateUI();
    Colony.Instance.SubscribeToUpdates(UpdateUI);
    Colony.Instance.SubscribeToMessages(Debug.Log);
  }
  private void UpdateUI() {
    UpdateResources();
    UpdateBuildings();
    SetText("Time/Tick", $"Tick: {Colony.Instance.CurrentTick}");
  }
  public void onNextTickClick() {
    Colony.Instance.NextTick();
  }
  public void onBuildBuilding(string buildingName) {
    Building building = Buildings.All.FirstOrDefault(b => b.Name == buildingName);
    if (building == null) {
      Debug.LogError($"Building '{buildingName}' not found.");
    } else {
      Colony.Instance.Build(building);
    }
  }
  private void UpdateResources() {
    SetText("Resources/Food", $"Food: {Colony.Instance.GetStockpile(Resources.Food)}");
    SetText("Resources/Water", $"Water: {Colony.Instance.GetStockpile(Resources.Water)}");
    SetText("Resources/Population", $"Population: {Colony.Instance.Population}");
    SetText("Resources/Power", $"Power: {Colony.Instance.PowerSurplus}");
  }
  private void UpdateBuilding(string id, Building building) {
    SetText($"Buildings/{id}/Count", $"{building.Name}s: {Colony.Instance.BuildingCount(building)}");
  }
  private void UpdateBuildings() {
    UpdateBuilding("Farms", Buildings.Farm);
    UpdateBuilding("HydroRecyclers", Buildings.HydroRecycler);
    UpdateBuilding("Generators", Buildings.Generator);
  }
  private void SetText(string name, string text) {
    TextMeshProUGUI tmp = canvas.transform
      .Find($"Panel/{name}").gameObject
      .GetComponent<TextMeshProUGUI>();
    tmp.SetText(text);
  }
}