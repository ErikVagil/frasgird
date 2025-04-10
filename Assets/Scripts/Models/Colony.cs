using System.Collections.Generic;
using System.Diagnostics;
public class Colony {
  public static Colony Instance { get; set; } = new Colony();
  public int PowerSurplus { get; private set; }
  public int Population { get; private set; }
  public int CurrentTick { get; private set; }
  private Dictionary<Good, int> stockpiles = new ();
  // private Dictionary<Building, int> buildings = new ();
  public delegate void ColonyUpdate();
  public delegate void DisplayMessage(string message);
  private ColonyUpdate onUpdate;
  public DisplayMessage displayMessage;
  public ColonyMap Map { get; private set; }
  public Colony() {
    this.CurrentTick = 0;
    Map = new ColonyMap();
    ModifyStockpile(Goods.Food, 20);
    ModifyStockpile(Goods.Water, 20);
    this.Population = 100;
    this.PowerSurplus = 0;

    // Build(Buildings.Generator);
  }

  public int GetStockpile(Good good) {
    if (!stockpiles.ContainsKey(good)) {
      return 0;
    } else {
      return stockpiles[good];
    }
  }

  public void ModifyStockpile(Good good, int amount) {
    if (!stockpiles.ContainsKey(good)) {
      stockpiles.Add(good, amount);
    } else {
      stockpiles[good] += amount;
    }
  }

  public void Build(BuildingPlot plot, Building building) {
    if (PowerSurplus + (plot.Building?.PowerConsumption ?? 0) < building.PowerConsumption) {
      displayMessage?.Invoke("Not enough power.");
      return;
    }
    
    if (plot.Building != null) {
      // displayMessage?.Invoke("plot not empty");
      Demolish(plot);
    }
    plot.Build(building);

    PowerSurplus -= building.PowerConsumption;
    onUpdate?.Invoke();
  }

  public void Demolish(BuildingPlot plot) {
    if (plot.Building == null ) {
      displayMessage?.Invoke("plot already empty");
      return;
    } /* else if (PowerSurplus + plot.Building.PowerConsumption < 0) {
      displayMessage?.Invoke("would result in negative power");
      return;
    } */

    PowerSurplus += plot.Building.PowerConsumption;
    plot.Demolish();
    onUpdate?.Invoke();
  }

  // public int BuildingCount(Building building) {
  //   if (!this.buildings.ContainsKey(building)) {
  //     return 0;
  //   } else {
  //     return this.buildings[building];
  //   }
  // }

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

  public void BuildingTick() {
    // foreach (var b in this.buildings) {
    //   for (int i = 0; i < b.Value; i++) {
    //     b.Key.OnTick();
    //   }
    // }
    foreach (var plot in Map.AllPlots) {
      displayMessage(plot.Building?.Name);
      plot.Building?.OnTick();
    }
  }

  public void NextTick() {
    BuildingTick();
    PopConsumption();

    CurrentTick++;
    onUpdate?.Invoke();
  }
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