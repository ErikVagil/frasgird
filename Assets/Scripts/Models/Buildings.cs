using System.Collections.Generic;
public static class Buildings {
  private static List<Building> allBuildings = new ();
  public static IEnumerable<Building> All { get => allBuildings; }
  private static Building CreateBuilding(string name, int power) {
    Building building = new Building(name, power);
    allBuildings.Add(building);
    return building;
  }
  public static readonly Building Farm = CreateBuilding("Farm", 2);
  public static readonly Building HydroRecycler = CreateBuilding("Hydro-Recycler", 3);
  public static readonly Building Generator = CreateBuilding("Generator", -5);
}