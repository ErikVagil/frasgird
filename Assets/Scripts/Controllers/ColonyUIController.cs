using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UIElements;
public class ColonyUIController : MonoBehaviour {
  public static ColonyUIController Instance { get; private set; }
  // public GameObject canvas;
  public GameObject colonyUI;
  public GameObject plotUI;
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    } // else error?

    UpdateUI();
    Colony.Instance.SubscribeToUpdates(UpdateUI);
    Colony.Instance.SubscribeToMessages(Debug.Log);
    CleanupManager.Instance.SubscribeToCleanup(Cleanup);
    LinkButtons();
    HidePlotMenu();
  }
  private void LinkButtons() {
    GetVisualElement<Button>(colonyUI, "NextTick").RegisterCallback<ClickEvent>(OnNextTickClick);
  }
  private void LinkPlotButtons() {
    GetVisualElement<Button>(plotUI, "Demolish").RegisterCallback((ClickEvent e) => {Debug.Log("HELLO");});
    GetVisualElement<Button>(plotUI, "BuildGenerator").RegisterCallback((ClickEvent e) => { onBuildBuilding("Generator"); });
    GetVisualElement<Button>(plotUI, "BuildFarm").RegisterCallback((ClickEvent e) => onBuildBuilding("Farm"));
    GetVisualElement<Button>(plotUI, "BuildHydroRecycler").RegisterCallback((ClickEvent e) => onBuildBuilding("Hydro-Recycler"));
  }
  private void UpdateUI() {
    UpdateResources();
    // UpdateBuildings();
    SetText(colonyUI, "Tick", $"Tick: {Colony.Instance.CurrentTick}");
  }
  private void OnNextTickClick(ClickEvent e) {
    Colony.Instance.NextTick();
  }
  public void onBuildBuilding(string buildingName) {
    Building building = Buildings.All.FirstOrDefault(b => b.Name == buildingName);
    if (building == null) {
      Debug.LogError($"Building '{buildingName}' not found.");
    } else {
      var plot = MouseController.Instance.SelectedPlot;
      if (plot == null) {
        Debug.LogError("No Selected Plot");
        return;
      }
      Colony.Instance.Build(plot.BuildingPlot, building);

    }
  }
  private void UpdateResources() {
    SetText(colonyUI, "Food", $"Food: {Colony.Instance.GetStockpile(Goods.Food)}");
    SetText(colonyUI, "Water", $"Water: {Colony.Instance.GetStockpile(Goods.Water)}");
    SetText(colonyUI, "Population", $"Population: {Colony.Instance.Population}");
    SetText(colonyUI, "Power", $"Power: {Colony.Instance.PowerSurplus}");
  }
  // private void UpdateBuilding(string id, Building building) {
  //   SetText($"Buildings/{id}/Count", $"{building.Name}s: {Colony.Instance.BuildingCount(building)}");
  // }
  // private void UpdateBuildings() {
  //   UpdateBuilding("Farms", Buildings.Farm);
  //   UpdateBuilding("HydroRecyclers", Buildings.HydroRecycler);
  //   UpdateBuilding("Generators", Buildings.Generator);
  // }

  private void SetText(GameObject ui, string name, string text) {
    GetVisualElement<Label>(ui, name).text = text;
  }
  private E GetVisualElement<E>(GameObject ui, string name) where E : VisualElement {
    return ui.GetComponent<UIDocument>().rootVisualElement.Query<E>(name: name).First();
  }
  public void ShowPlotMenu() {
    plotUI.SetActive(true);
    LinkPlotButtons();
  }
  public void HidePlotMenu() {
    plotUI.SetActive(false);
  }
  private void Cleanup() {
    Colony.Instance.UnsubscribeToUpdates(UpdateUI);
  }
}