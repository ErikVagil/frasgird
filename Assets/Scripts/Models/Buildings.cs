using System.Collections.Generic;
using System.Linq;
public static class Buildings {
  private static List<Building> allBuildings = new ();
  public static IEnumerable<Building> All { get => allBuildings; }
  private static Building RegisterBuilding(Building building) {
    if (allBuildings.Any(x => x.Name == building.Name)) {
      // error; duplicate name
    }
    allBuildings.Add(building);
    return building;
  }
  public static readonly Building Farm = RegisterBuilding(
    new Factory("Farm",
                2,
                BuildingRequirement.IsEmpty,
                BuildingRequirement.None,
                new [] { (Goods.Water, 2) },
                new [] { (Goods.Food, 5) }));
  public static readonly Building HydroRecycler = RegisterBuilding(
    new Factory("Hydro-Recycler",
                3,
                BuildingRequirement.IsEmpty,
                BuildingRequirement.None,
                null,
                new [] { (Goods.Water, 5) }));
  public static readonly Building Generator = RegisterBuilding(
    new Building( "Generator",
                  -5,
                  new BuildingRequirement.Any(
                    BuildingRequirement.IsEmpty,
                    new BuildingRequirement.BaseBuilding("Steam Turbine")
                  ),
                  BuildingRequirement.None));

  public static readonly Building SteamTurbine = RegisterBuilding(
    new Factory("Steam Turbine",
                -10,
                new BuildingRequirement.BaseBuilding("Generator"),
                BuildingRequirement.None,
                new [] { (Goods.Water, 2) },
                null));
}