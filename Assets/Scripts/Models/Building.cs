using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Building class used to create buildings. Building objects represent
/// types of building. They don't represent individual instances of a building,
/// those are represented by BuildingPlots
/// </summary>
public class Building {
  /// <summary>
  /// Name of the Building. Used sometimes to search for it
  /// in the static list of buildings in Buildings.cs
  /// </summary>
  public string Name { get; private set; }

  /// <summary>
  /// How much burden does this put on the power grid?
  /// </summary>
  public int PowerConsumption { get; private set; }

  /// <summary>
  /// Core Building Requirements. If the CoreRequirement isn't met,
  /// then the building will not show up in the plotUI build menu.
  /// </summary>
  public BuildingRequirement CoreRequirement;

  /// <summary>
  /// Additional Building Requirements. If the requirement is not met,
  /// the button in the plotUI build menu will be disabled.
  /// 
  /// Two building requirements, power and resources needed,
  /// are automatically added and don't need to be passed in to
  /// the constructor.
  /// </summary>
  public BuildingRequirement AdditionalRequirement;

  /// <summary>
  /// This is just used for aesthetic reasons. The text (upgrade, build,
  /// demolish, etc.) used in the build menu depends on the BuildingLevel.
  /// more information in GetBuildDescription()
  /// </summary>
  public int BuildingLevel { get; private set; }
  
  /// <summary>
  /// An array of tuples, representing the costs required to build
  /// the building. The good, and how much is needed.
  /// </summary>
  public (Good good, int amount)[] BuildingCosts => buildingCosts.ToArray();
  private (Good good, int amount)[] buildingCosts;
  
  public Building(string name,
                  int powerConsumption,
                  (Good good, int amount)[] buildingCosts,
                  BuildingRequirement coreRequirement,
                  BuildingRequirement additionalRequirement,
                  int buildingLevel) {
    Name = name;
    PowerConsumption = powerConsumption;
    this.buildingCosts = buildingCosts ?? new (Good, int)[] {};
    CoreRequirement = coreRequirement;
    AdditionalRequirement = new BuildingRequirement.All(
      new BuildingRequirement.PowerRequirement(powerConsumption),
      new BuildingRequirement.BuildingCost(buildingCosts),
      additionalRequirement
    );
    BuildingLevel = buildingLevel;
  }

  /// <summary>
  /// This represents the per-tick building logic. The Factory
  /// subclass uses this to generate resources.
  /// </summary>
  public virtual void OnTick() {
    // nothing
  }

  /// <summary>
  /// Returns the string used to show the action taken to go
  /// from one building to another. For example, going from "Farm" to
  /// "Empty Plot" will say "Demolish Farm" and going from "Generator" to
  /// "Steam Turbine" will say "Upgrade to Steam Turbine".
  /// 
  /// This relies on the BuildingLevel variable.
  /// </summary>
  /// <param name="from">The building that already exists.</param>
  /// <param name="to">The possible building to replace the current one with.</param>
  /// <returns></returns>
  public static string GetBuildDescription(Building from, Building to) {
    if (to.BuildingLevel == 0) {
      return $"Demolish {from.Name}";
    } else if (from.BuildingLevel == 0) {
      return $"Build {to.Name}";
    } else if (from.BuildingLevel > to.BuildingLevel) {
      return $"Downgrade to {to.Name}";
    } else {
      return $"Upgrade to {to.Name}";
    }
  }
}


