using System.Collections.Generic;
using System.Linq;

public abstract class BuildingRequirement {
  public abstract bool IsMet(BuildingPlot plot);
  public static BuildingRequirement None { get; private set; } = new FunctionalRequirement((BuildingPlot plot) => true);
  public static BuildingRequirement IsEmpty { get; private set; } = new FunctionalRequirement((BuildingPlot plot) => plot.Building == null);
  public class BaseBuilding : BuildingRequirement {
    private string RequiredBuilding { get; set; }
    public BaseBuilding(string required) {
      RequiredBuilding = required;
    }
    public override bool IsMet(BuildingPlot plot) {
      return plot.Building?.Name == RequiredBuilding;
    }
  }

  public class Any : BuildingRequirement {
    private BuildingRequirement[] requirements;
    public Any(params BuildingRequirement[] requirements) {
      this.requirements = requirements;
    }

    public override bool IsMet(BuildingPlot plot) {
      return requirements.Any(x => x.IsMet(plot));
    }
  }

  public class All : BuildingRequirement {
    private BuildingRequirement[] requirements;
    public All(params BuildingRequirement[] requirements) {
      this.requirements = requirements;
    }

    public override bool IsMet(BuildingPlot plot) {
      return requirements.All(x => x.IsMet(plot));
    }
  }

  public class PowerRequirement : BuildingRequirement {
    private int power;
    public PowerRequirement(int power) {
      this.power = power;
    }

    public override bool IsMet(BuildingPlot plot) {
      return Colony.Instance.PowerSurplus + (plot.Building?.PowerConsumption ?? 0) >= power;
    }
  }

  private class FunctionalRequirement : BuildingRequirement {
    public delegate bool RequirementFunction(BuildingPlot plot);
    private RequirementFunction func;
    public FunctionalRequirement(RequirementFunction func) {
      this.func = func;
    }
    public override bool IsMet(BuildingPlot plot) => func(plot);
  }
}

