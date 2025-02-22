using System.Collections.Generic;
public class Colony {
  public static Colony Instance { get; set; } = new Colony();
  public int PowerSurplus { get; private set; }
  public int Population { get; private set; }
  public int CurrentTick { get; private set; }
  private Dictionary<Resource, int> stockpiles = new ();
  private Dictionary<Building, int> buildings = new ();
  public delegate void ColonyUpdate();
  public delegate void DisplayMessage(string message);
  private ColonyUpdate onUpdate;
  public DisplayMessage displayMessage;
  public Colony() {
    this.CurrentTick = 0;
    ModifyStockpile(Resources.Food, 20);
    ModifyStockpile(Resources.Water, 20);
    this.Population = 100;
    this.PowerSurplus = 0;

    Build(Buildings.Generator);
  }

  public int GetStockpile(Resource resource) {
    if (!stockpiles.ContainsKey(resource)) {
      return 0;
    } else {
      return stockpiles[resource];
    }
  }

  public void ModifyStockpile(Resource resource, int amount) {
    if (!stockpiles.ContainsKey(resource)) {
      stockpiles.Add(resource, amount);
    } else {
      stockpiles[resource] += amount;
    }
  }

  public void Build(Building building) {
    if (PowerSurplus < building.PowerConsumption) {
      displayMessage?.Invoke("Not enough power.");
      return;
    }
    if (!this.buildings.ContainsKey(building)) {
      this.buildings.Add(building, 1);
    } else {
      this.buildings[building]++;
    }

    this.PowerSurplus -= building.PowerConsumption;
    onUpdate?.Invoke();
  }

  public int BuildingCount(Building building) {
    if (!this.buildings.ContainsKey(building)) {
      return 0;
    } else {
      return this.buildings[building];
    }
  }

  public void PopConsumption() {
    int foodNeeded = Population / 10;
    int waterNeeded = Population / 10;
    bool malnourishment = false;
    int food = GetStockpile(Resources.Food);
    int water = GetStockpile(Resources.Water);
    if (food < foodNeeded) {
      malnourishment = true;
      ModifyStockpile(Resources.Food, -food);
    } else {
      ModifyStockpile(Resources.Food, -foodNeeded);
    }
    if (water < waterNeeded) {
      malnourishment = true;
      ModifyStockpile(Resources.Water, -water);
    } else {
      ModifyStockpile(Resources.Water, -waterNeeded);
    }
    if (malnourishment) {
      Population = (int)(0.95 * Population);
    } else {
      Population = (int)(1.01 * Population);
    }
  }

  public void BuildingTick() {
    foreach (var b in this.buildings) {
      for (int i = 0; i < b.Value; i++) {
        b.Key.OnTick();
      }
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
  public void SubscribeToMessages(DisplayMessage callback) {
    displayMessage += callback;
  }
}