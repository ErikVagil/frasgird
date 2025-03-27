public class Building {
  public string Name { get; private set; }
  public int PowerConsumption { get; private set; }
  public BuildingRequirement CoreRequirement;
  public BuildingRequirement AdditionalRequirement;
  public Building(string name, int powerConsumption, BuildingRequirement coreRequirement, BuildingRequirement additionalRequirement) {
    Name = name;
    PowerConsumption = powerConsumption;
    CoreRequirement = coreRequirement;
    AdditionalRequirement = new BuildingRequirement.All(
      new BuildingRequirement.PowerRequirement(powerConsumption),
      additionalRequirement
    );
  }
  public virtual void OnTick() {
    // nothing
  }
}


