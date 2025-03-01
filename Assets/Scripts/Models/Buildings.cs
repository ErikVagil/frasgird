using System.Collections.Generic;
public static class Buildings {
  private static List<Building> allBuildings = new ();
  public static IEnumerable<Building> All { get => allBuildings; }
  private static Building RegisterBuilding(Building building) {
    allBuildings.Add(building);
    return building;
  }
  public static readonly Building Farm = RegisterBuilding(
    new Factory("Farm",
                2,
                new [] { (Goods.Water, 2) },
                new [] { (Goods.Food, 5) }));
  public static readonly Building HydroRecycler = RegisterBuilding(
    new Factory("Hydro-Recycler",
                3,
                null,
                new [] { (Goods.Water, 5) }));
  public static readonly Building Generator = RegisterBuilding(new Building("Generator", -5));
}