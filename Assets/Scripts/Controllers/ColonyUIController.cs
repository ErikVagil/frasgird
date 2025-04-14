using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// This class isn't the cleanest. It interfaces with the
/// Unity UI documents in the scene, handling button presses and
/// label updates.
/// </summary>
public class ColonyUIController : MonoBehaviour {
  public static ColonyUIController Instance { get; private set; }
  public GameObject colonyUI;
  public GameObject plotUI;
  public GameObject tooltip;
  public StyleSheet uss;

  // These lists are for the ListView elements
  private List<string> goodsList;
  private List<(string, Building)> buildingOptionsList;

  /// <summary>
  /// Sets up the various ui documents
  /// </summary>
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
  /// <summary>
  /// Moves the tooltip to the current mouse location so long as
  /// it's currently visible
  /// </summary>
  void Update()
  {
    if (IsTooltipVisible()) {
      var pos = RuntimePanelUtils.ScreenToPanel(GetVisualElement<VisualElement>(tooltip, "Panel").panel, Input.mousePosition);
      pos.y = Screen.height - pos.y;
      pos.x += 25;
      GetVisualElement<VisualElement>(tooltip, "Panel").transform.position = pos;
    }
  }
  /// <summary>
  /// Updates the colony ui once it finishes calculating how large
  /// it's supposed to be relative to the screen. Sets up the
  /// ListView logic for goods.
  /// </summary>
  /// <param name="e">Contains the updated size of the colonyUI panel.</param>
  private void ScaleColonyUI(GeometryChangedEvent e) {
    int fontSize = (int)(e.newRect.height * 0.035);
    GetVisualElement<Label>(colonyUI, "Population").style.fontSize = fontSize;
    GetVisualElement<Label>(colonyUI, "Power").style.fontSize = fontSize;
    GetVisualElement<Label>(colonyUI, "Tick").style.fontSize = fontSize;
    GetVisualElement<Button>(colonyUI, "NextTick").style.fontSize = fontSize;
    var listView = GetVisualElement<ListView>(colonyUI, "GoodsList");
    listView.fixedItemHeight = (int)(e.newRect.height * 0.1);
    listView.bindItem = (el, i) => {
      if (i < 0) {
        return;
      }
      Good good = Goods.GetGoodFromID(i);
      Label l = el as Label;
      l.style.fontSize = fontSize;
      l.text = $"{good.Name}: {Colony.Instance.GetStockpile(good)}";
      l.AddToClassList("labelListView");
      l.styleSheets.Add(uss);
    };
    listView.RefreshItems();
  }
  /// <summary>
  /// Updates the plot ui once it finishes calculating how large
  /// it's supposed to be relative to the screen. Uses a ListView
  /// to organize the buttons for construction and destruction of buildings.
  /// </summary>
  /// <param name="e">Contains the updated size of the plotUI panel.</param>
  private void ScalePlotUI(GeometryChangedEvent e) {
    int fontSize = (int)(e.newRect.height * 0.03);
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
      b.AddToClassList("plotBuildingOptionButton");
      b.text = pair.Item1;
      b.style.unityTextAlign = TextAnchor.MiddleCenter;
      b.style.whiteSpace = WhiteSpace.Normal;
      b.RegisterCallback((ClickEvent e) => onBuildBuilding(pair.Item2.Name));
      b.RegisterCallback((MouseOverEvent e) => ShowTooltip(pair.Item2));
      b.RegisterCallback((MouseOutEvent e) => HideTooltip());
      
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

  /// <summary>
  /// Constructs the list of buildings that can be built/destroyed,
  /// and sets the plotUI to show them.
  /// </summary>
  private void UpdatePlotUI() {
    var plot = MouseController.Instance.SelectedPlot;
    if (plot == null) {
      Debug.LogError("no plot selected");
      return;
    }
    SetText(plotUI, "Building", plot.BuildingPlot.Building.Name);

    buildingOptionsList.Clear();
    foreach (Building building in Buildings.All) {
      if (building.CoreRequirement.IsMet(plot.BuildingPlot)) {
        buildingOptionsList.Add((
          Building.GetBuildDescription(plot.BuildingPlot.Building, building),
          building));
      }
    }

    GetVisualElement<ListView>(plotUI, "OptionsList").Rebuild();
  }

  /// <summary>
  /// Updates the various UIs to display information; is called in this
  /// class and also used to subscribe to Colony to display changes in information.
  /// </summary>
  private void UpdateUI() {
    UpdateResources();
    SetText(colonyUI, "Tick", $"Tick: {Colony.Instance.CurrentTick}");
    if (IsPlotMenuVisible()) {
      UpdatePlotUI();
    }
    GetVisualElement<ListView>(colonyUI, "GoodsList").RefreshItems();
  }

  /// <summary>
  /// Called by the button advancing the colony tick.
  /// </summary>
  /// <param name="e">ClickEvent; ignored</param>
  private void OnNextTickClick(ClickEvent e) {
    Colony.Instance.NextTick();
  }

  /// <summary>
  /// Called by the buttons in the plotUI that construct and
  /// destroy buildings.
  /// </summary>
  /// <param name="buildingName">The name of the Building to build, in Buildings.cs.</param>
  public void onBuildBuilding(string buildingName) {
    HideTooltip();
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

  /// <summary>
  /// Updates the labels for resources that aren't Goods
  /// (population and power for now)
  /// </summary>
  private void UpdateResources() {
    SetText(colonyUI, "Population", $"Population: {Colony.Instance.Population}");
    SetText(colonyUI, "Power", $"Power: {Colony.Instance.PowerSurplus}");
  }

  /// <summary>
  /// Shorthand for setting a label's text. Probably not necessary,
  /// but was more necessary in earlier code.
  /// </summary>
  /// <param name="ui">The UI Document to search in.</param>
  /// <param name="name">The name of the Label in the UI Document tree.</param>
  /// <param name="text">The text to set the label to.</param>
  private void SetText(GameObject ui, string name, string text) {
    GetVisualElement<Label>(ui, name).text = text;
  }

  /// <summary>
  /// Returns the visual element requested from the UI Document indicated.
  /// </summary>
  /// <typeparam name="E">The type of VisualElement you expect.</typeparam>
  /// <param name="ui">The UI Document to search in.</param>
  /// <param name="name">The name of the VisualElement to return.</param>
  /// <returns></returns>
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

  /// <summary>
  /// disconnects from subscription events; used for when the mode changes
  /// 
  /// </summary>
  private void Cleanup() {
    Colony.Instance.UnsubscribeToUpdates(UpdateUI);
  }

  private void ShowTooltip(Building target) {
    var plot = MouseController.Instance.SelectedPlot;
    if (plot == null) {
      Debug.LogError("no plot selected");
      return;
    }

    tooltip.SetActive(true);
    var panel = GetVisualElement<VisualElement>(tooltip, "Panel");
    var reqs = target.AdditionalRequirement.Requirements(plot.BuildingPlot);
    if (!reqs.Any()) {
      HideTooltip();
      return;
    }

    int maxWidth = 0;
    int labelHeight = 30;
    int fontSize = 25;
    foreach ((string text, bool met) in reqs) {
      if (text.Length > maxWidth) {
        maxWidth = text.Length;
      }
      Label label = new Label();
      panel.Add(label);
      label.text = $"<b>{text}</b>";
      Color darkGreen = Color.green;
      darkGreen.r *= 0.8f;
      darkGreen.g *= 0.8f;
      darkGreen.b *= 0.8f;
      label.style.color = met ? darkGreen : Color.red;
      label.style.fontSize = fontSize;
      label.style.height = labelHeight;
    }

    panel.style.height = labelHeight * reqs.Count() + 50;
    panel.style.width = maxWidth * fontSize;
  }

  private void HideTooltip() {
    tooltip.SetActive(false);
  }

  private bool IsTooltipVisible() {
    return tooltip.activeSelf;
  }
}