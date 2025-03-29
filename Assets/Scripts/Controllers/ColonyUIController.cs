using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;
public class ColonyUIController : MonoBehaviour {
  public static ColonyUIController Instance { get; private set; }
  // public GameObject canvas;
  public GameObject colonyUI;
  public GameObject plotUI;
  private List<string> goodsList;
  // private delegate void onOptionBuild();
  private List<(string, Building)> buildingOptionsList;
  void OnEnable() {
    if (Instance == null) {
      Instance = this;
    } // else error?

    HidePlotMenu();
    Colony.Instance.SubscribeToUpdates(UpdateUI);
    Colony.Instance.SubscribeToMessages(Debug.Log);
    CleanupManager.Instance.SubscribeToCleanup(Cleanup);
    LinkButtons();
    
    goodsList = new();
    foreach (Good good in Goods.All) {
      goodsList.Add(good.Name);
    }
    var listView = GetVisualElement<ListView>(colonyUI, "GoodsList");
    listView.itemsSource = goodsList;
    listView.selectionType = SelectionType.None;
    listView.makeItem = () => new Label();

    buildingOptionsList = new ();
    listView = GetVisualElement<ListView>(plotUI, "OptionsList");
    listView.itemsSource = buildingOptionsList;
    listView.selectionType = SelectionType.None;
    listView.makeItem = () => new Button();
    
    GetVisualElement<VisualElement>(colonyUI, "Panel").RegisterCallback<GeometryChangedEvent>(ScaleColonyUI);
    GetVisualElement<VisualElement>(plotUI, "Panel").RegisterCallback<GeometryChangedEvent>(ScalePlotUI);
    UpdateUI();
  }
  private void ScaleColonyUI(GeometryChangedEvent e) {
    int fontSize = (int)(e.newRect.height * 0.05);
    GetVisualElement<Label>(colonyUI, "Population").style.fontSize = fontSize;
    GetVisualElement<Label>(colonyUI, "Power").style.fontSize = fontSize;
    GetVisualElement<Label>(colonyUI, "Tick").style.fontSize = fontSize;
    GetVisualElement<Button>(colonyUI, "NextTick").style.fontSize = fontSize;
    var listView = GetVisualElement<ListView>(colonyUI, "GoodsList");
    listView.fixedItemHeight = (int)(e.newRect.height * 0.1);
    listView.bindItem = (el, i) => {
      
      Good good = Goods.GetGoodFromID(i);
      Label l = el as Label;
      l.style.fontSize = fontSize;
      l.text = $"{good.Name}: {Colony.Instance.GetStockpile(good)}";
    };
    listView.RefreshItems();
  }
  private void ScalePlotUI(GeometryChangedEvent e) {
    int fontSize = (int)(e.newRect.height * 0.04);
    GetVisualElement<Label>(plotUI, "Building").style.fontSize = fontSize;
    var listView = GetVisualElement<ListView>(plotUI, "OptionsList");
    listView.fixedItemHeight = (int)(e.newRect.height * 0.1);
    listView.bindItem = (el, i) => {
      if (!(i < buildingOptionsList.Count && i >= 0)) {
        return;
      }
      var pair = buildingOptionsList[i];
      Button b = el as Button;
      b.style.fontSize = fontSize;
      b.text = pair.Item1;
      b.RegisterCallback((ClickEvent e) => onBuildBuilding(pair.Item2.Name));
      
      if (IsPlotMenuVisible()) {
        BuildingPlot plot = MouseController.Instance.SelectedPlot.BuildingPlot;
        if (!pair.Item2?.AdditionalRequirement.IsMet(plot) ?? false) {
          b.SetEnabled(false);
        }
      }
    };
    listView.Rebuild();
  }
  private void LinkButtons() {
    GetVisualElement<Button>(colonyUI, "NextTick").RegisterCallback<ClickEvent>(OnNextTickClick);
  }
  private void UpdatePlotUI() {
    var plot = MouseController.Instance.SelectedPlot;
    if (plot == null) {
      Debug.LogError("no plot selected");
      return;
    }
    // bool isEmpty = plot.BuildingPlot.Building == null;
    // SetText(plotUI, "Building", isEmpty ? "Empty" : plot.BuildingPlot.Building.Name);
    SetText(plotUI, "Building", plot.BuildingPlot.Building.Name);

    buildingOptionsList.Clear();
    // if (!isEmpty) {
    //   buildingOptionsList.Add(("Demolish", null));
      
    // }
    foreach (Building building in Buildings.All) {
      if (building.CoreRequirement.IsMet(plot.BuildingPlot)) {
        buildingOptionsList.Add((
          Building.GetBuildDescription(plot.BuildingPlot.Building, building),
          building));
      }
    }

    GetVisualElement<ListView>(plotUI, "OptionsList").Rebuild();
  }
  private void UpdateUI() {
    UpdateResources();
    SetText(colonyUI, "Tick", $"Tick: {Colony.Instance.CurrentTick}");
    if (IsPlotMenuVisible()) {
      UpdatePlotUI();
    }
    GetVisualElement<ListView>(colonyUI, "GoodsList").RefreshItems();
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
  // public void Demolish() {
  //   var plot = MouseController.Instance.SelectedPlot;
  //     if (plot == null) {
  //       Debug.LogError("No Selected Plot");
  //       return;
  //     }
  //     Colony.Instance.DemolishLogic(plot.BuildingPlot);
  // }
  private void UpdateResources() {
    SetText(colonyUI, "Population", $"Population: {Colony.Instance.Population}");
    SetText(colonyUI, "Power", $"Power: {Colony.Instance.PowerSurplus}");
  }

  private void SetText(GameObject ui, string name, string text) {
    GetVisualElement<Label>(ui, name).text = text;
  }
  private E GetVisualElement<E>(GameObject ui, string name) where E : VisualElement {
    return ui.GetComponent<UIDocument>().rootVisualElement.Query<E>(name: name).First();
  }
  public void ShowPlotMenu() {
    plotUI.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
    UpdateUI();
  }
  public void HidePlotMenu() {
    plotUI.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
  }
  private bool IsPlotMenuVisible() {
    return plotUI.GetComponent<UIDocument>().rootVisualElement.style.display == DisplayStyle.Flex;
  }
  private void Cleanup() {
    Colony.Instance.UnsubscribeToUpdates(UpdateUI);
  }
}