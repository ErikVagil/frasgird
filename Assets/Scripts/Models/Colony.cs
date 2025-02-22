using System.Collections.Generic;
public class Colony {
  public static Colony Instance { get; set; } = new Colony();
  public int Food { get; private set; }
  public int Water { get; private set; }
  public int PowerSurplus { get; private set; }
  public int Population { get; private set; }
  public int CurrentTick { get; private set; }

  private Dictionary<Building, int> buildings = new ();
  public delegate void ColonyUpdate();
  public delegate void DisplayMessage(string message);
  private ColonyUpdate onUpdate;
  public DisplayMessage displayMessage;
  public Colony() {
    this.CurrentTick = 0;
    this.Food = 20;
    this.Water = 20;
    this.Population = 100;
    this.PowerSurplus = 0;

    Build(Buildings.Generator);
  }

  public void Build(Building building) {
    if (PowerSurplus < building.PowerConsumption) {
      displayMessage?.Invoke("Not enough power.");
      return;
    }
    if (!this.buildings.ContainsKey(building)) {
      this.buildings.Add(building, 1);
    } else {
      this.buildings[building] = this.buildings[building] + 1;
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

  public void NextTick() {
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