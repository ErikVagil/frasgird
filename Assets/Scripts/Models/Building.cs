public class Building {
  public string Name { get; private set; }
  public int PowerConsumption { get; private set; }
  public BuildingRequirement CoreRequirement;
  public BuildingRequirement AdditionalRequirement;
  public int BuildingLevel { get; private set; }
  public Building(string name, int powerConsumption, BuildingRequirement coreRequirement, BuildingRequirement additionalRequirement, int buildingLevel) {
    Name = name;
    PowerConsumption = powerConsumption;
    CoreRequirement = coreRequirement;
    AdditionalRequirement = new BuildingRequirement.All(
      new BuildingRequirement.PowerRequirement(powerConsumption),
      additionalRequirement
    );
    BuildingLevel = buildingLevel;
  }
  public virtual void OnTick() {
    // nothing
  }

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


