using System.Collections.Generic;
using System.Linq;
public static class Buildings {
  private static List<Building> allBuildings = new ();
  public static IEnumerable<Building> All { get => allBuildings; }

  /// <summary>
  /// Use this function when declaring a new building to
  /// make sure it gets added to the list.
  /// </summary>
  /// <param name="building"></param>
  /// <returns></returns>
  private static Building RegisterBuilding(Building building) {
    if (allBuildings.Any(x => x.Name == building.Name)) {
      // error; duplicate name
    }
    allBuildings.Add(building);
    return building;
  }

  /// <summary>
  /// Empty Plot. The default building in each BuildingPlot.
  /// </summary>
  public static readonly Building Empty = RegisterBuilding(
    new Building("Empty Plot",
      0,
      null,
      new BuildingRequirement.Not(new BuildingRequirement.BaseBuilding("Empty Plot")),
      BuildingRequirement.None,
      0
    )
  );

  public static readonly Building Farm = RegisterBuilding(
    new Factory("Farm",
                2,
                null,
                BuildingRequirement.IsEmpty,
                BuildingRequirement.None,
                1,
                new [] { (Goods.Water, 2) },
                new [] { (Goods.Food, 5) }));

  public static readonly Building HydroRecycler = RegisterBuilding(
    new Factory("Hydro-Recycler",
                3,
                null,
                BuildingRequirement.IsEmpty,
                BuildingRequirement.None,
                1,
                null,
                new [] { (Goods.Water, 5) }));

  /// <summary>
  /// Generator, costs negative power (meaning it provides it)
  /// 
  /// One thing in the future someone might want to change is manual
  /// assignment of required buildings. Right now, the Generator requires
  /// either an empty plot or a Steam Turbine (its upgrade) to build;
  /// there's no logic telling it that upgrading to a steam turbine
  /// should imply being able to downgrade to a generator.
  /// </summary>
  public static readonly Building Generator = RegisterBuilding(
    new Building( "Generator",
                  -5,
                  new [] { (Goods.Steel, 5) },
                  new BuildingRequirement.Any(
                    BuildingRequirement.IsEmpty,
                    new BuildingRequirement.BaseBuilding("Steam Turbine")
                  ),
                  BuildingRequirement.None,
                  1));

  /// <summary>
  /// Mostly a proof of concept example building upgrade.
  /// Consumes 2 water every turn; with the way the code
  /// currently works a lack of water won't shut off the power, but
  /// it will consume as much as it can, and you don't want to run out
  /// of water anyway.
  /// </summary>
  public static readonly Building SteamTurbine = RegisterBuilding(
    new Factory("Steam Turbine",
                -10,
                new [] { (Goods.Steel, 3) },
                new BuildingRequirement.BaseBuilding("Generator"),
                BuildingRequirement.None,
                2,
                new [] { (Goods.Water, 2) },
                null));
}