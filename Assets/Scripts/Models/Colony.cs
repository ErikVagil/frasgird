using System.Collections.Generic;
public class Colony {
  public static Colony Instance { get; set; } = new Colony();
  public int Food { get; private set; }
  public int Water { get; private set; }
  public int PowerSurplus { get; private set; }
  public int Population { get; private set; }

  private Dictionary<Building, int> buildings = new ();
  public Colony() {
    this.Food = 0;
    this.Water = 0;
    this.Population = 100;
    this.PowerSurplus = 0;

    Build(Buildings.Generator);
  }

  public void Build(Building building) {
    if (!this.buildings.ContainsKey(building)) {
      this.buildings.Add(building, 1);
    } else {
      this.buildings[building] = this.buildings[building] + 1;
    }

    this.PowerSurplus -= building.PowerConsumption;
  }
}