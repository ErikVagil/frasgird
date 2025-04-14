using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// The heart of the player data, contains Frasgird's resources,
/// tiles, and logic for building buildings and manipulating goods.
/// </summary>
public class Colony {
  /// <summary>
  /// Singleton. Use this to access the one Colony from any script.
  /// </summary>
  public static Colony Instance { get; set; } = new Colony();

  /// <summary>
  /// How much surplus power does our grid currently provide? This is
  /// a single, positive value (it should never be negative), that will be
  /// reduced by building buildings that consume power, and increased by
  /// demolishing buildings and building buildings that produce power, etc.
  /// </summary>
  public int PowerSurplus { get; private set; }

  /// <summary>
  /// Frasgird's human population
  /// </summary>
  public int Population { get; private set; }

  /// <summary>
  /// The current Colony Tick. Each tick, resources get
  /// consumed and produced, and the population grows or shrinks.
  /// </summary>
  public int CurrentTick { get; private set; }

  /// <summary>
  /// The colony's stockpiles of Good type resources.
  /// Food, water, building materials, etc.
  /// </summary>
  private Dictionary<Good, int> stockpiles = new ();
  
  /*** Delegate types and event handlers ***/
  public delegate void ColonyUpdate();
  public delegate void DisplayMessage(string message);
  private ColonyUpdate onUpdate;
  // This decouples the message display system from unity.
  // Right now it just calls Debug.Log, but can also be used to additionally
  // log to files if we want.
  public DisplayMessage displayMessage;

  /// <summary>
  /// The ColonyMap, containing the rings of BuildingPlots
  /// </summary>
  public ColonyMap Map { get; private set; }
  public Colony() {
    this.CurrentTick = 0;
    Map = new ColonyMap();
    ModifyStockpile(Goods.Food, 20);
    ModifyStockpile(Goods.Water, 20);
    ModifyStockpile(Goods.Steel, 50);
    this.Population = 100;
    this.PowerSurplus = 0;

    // Build(Buildings.Generator);
  }

  /// <summary>
  ///  Returns the stockpile of the requested Good.
  /// </summary>
  /// <param name="good">Good object to request.</param>
  /// <returns>integer representing how much we have.</returns>
  public int GetStockpile(Good good) {
    if (!stockpiles.ContainsKey(good)) {
      return 0;
    } else {
      return stockpiles[good];
    }
  }

  /// <summary>
  /// Adds the amount of the given resource to the colony's stockpile.
  /// </summary>
  /// <param name="good"></param>
  /// <param name="amount">positive to add, negative to subtract</param>
  public void ModifyStockpile(Good good, int amount) {
    if (!stockpiles.ContainsKey(good)) {
      stockpiles.Add(good, amount);
    } else {
      stockpiles[good] += amount;
    }
  }

  /// <summary>
  /// Builds the given Building in the given BuildingPlot.
  /// Should return if the requirements aren't met; but in theory this
  /// method should never be called if they aren't met.
  /// </summary>
  /// <param name="plot"></param>
  /// <param name="building"></param>
  public void Build(BuildingPlot plot, Building building) {
    if (PowerSurplus + (plot.Building?.PowerConsumption ?? 0) < building.PowerConsumption) {
      displayMessage?.Invoke("Not enough power.");
      return;
    }
    foreach (var pair in building.BuildingCosts) {
      if (GetStockpile(pair.good) < pair.amount) {
        displayMessage?.Invoke($"Not enough {pair.good.Name}.");
        return;
      }
    }
    
    if (plot.Building != null) {
      DemolishLogic(plot);
    }
    plot.Build(building);

    PowerSurplus -= building.PowerConsumption;
    foreach (var pair in building.BuildingCosts) {
      ModifyStockpile(pair.good, -pair.amount);
    }

    onUpdate?.Invoke();
  }

  /// <summary>
  /// Refunds power consumption; mostly a holdover from previous code
  /// but could become useful again if we want to refund building materials.
  /// </summary>
  /// <param name="plot"></param>
  private void DemolishLogic(BuildingPlot plot) {
    if (plot.Building == null ) {
      displayMessage?.Invoke("plot already empty");
      return;
    }

    PowerSurplus += plot.Building.PowerConsumption;
  }

  /// <summary>
  /// Consumes food and water depending on population,
  /// and expands or shrinks the population if the population's needs
  /// are met, or not met, respectively.
  /// </summary>
  public void PopConsumption() {
    int foodNeeded = Population / 10;
    int waterNeeded = Population / 10;
    bool malnourishment = false;
    int food = GetStockpile(Goods.Food);
    int water = GetStockpile(Goods.Water);
    if (food < foodNeeded) {
      malnourishment = true;
      ModifyStockpile(Goods.Food, -food);
    } else {
      ModifyStockpile(Goods.Food, -foodNeeded);
    }
    if (water < waterNeeded) {
      malnourishment = true;
      ModifyStockpile(Goods.Water, -water);
    } else {
      ModifyStockpile(Goods.Water, -waterNeeded);
    }
    if (malnourishment) {
      Population = (int)(0.95 * Population);
    } else {
      Population = (int)(1.01 * Population);
    }
  }

  /// <summary>
  /// On each tick, will call each Building to allow it to produce goods
  /// or perform whatever other needs it wants to.
  /// </summary>
  public void BuildingTick() {
    foreach (var plot in Map.AllPlots) {
      plot.Building?.OnTick();
    }
  }

  /// <summary>
  /// Updates the current tick, and calls relevant methods.
  /// </summary>
  public void NextTick() {
    BuildingTick();
    PopConsumption();

    CurrentTick++;
    onUpdate?.Invoke();
  }

  /*** delegate subscriptions. ***/
  public void SubscribeToUpdates(ColonyUpdate callback) {
    onUpdate += callback;
  }
  public void UnsubscribeToUpdates(ColonyUpdate callback) {
    onUpdate -= callback;
  }
  public void SubscribeToMessages(DisplayMessage callback) {
    displayMessage += callback;
  }
  public void UnsubscribeToMessages(DisplayMessage callback) {
    displayMessage -= callback;
  }
}