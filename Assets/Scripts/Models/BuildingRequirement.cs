using System.Collections.Generic;
using System.Linq;

public abstract class BuildingRequirement {
  public abstract bool IsMet(BuildingPlot plot);
  public abstract IEnumerable<(string, bool)> Requirements(BuildingPlot plot);
  public static BuildingRequirement None { get; private set; } = new FunctionalRequirement((BuildingPlot plot) => true, null);
  public static BuildingRequirement IsEmpty { get; private set; } = new BaseBuilding("Empty Plot");
  public class BaseBuilding : BuildingRequirement {
    private string RequiredBuilding { get; set; }
    public BaseBuilding(string required) {
      RequiredBuilding = required;
    }
    public override bool IsMet(BuildingPlot plot) {
      return plot.Building?.Name == RequiredBuilding;
    }
    public override IEnumerable<(string, bool)> Requirements(BuildingPlot plot)
    {
      yield return (RequiredBuilding, IsMet(plot));
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

    public override IEnumerable<(string, bool)> Requirements(BuildingPlot plot)
    {
      return requirements.Select(x => x.Requirements(plot)).Aggregate((x, y) => x.Concat(y)).Select(x => ($"OPTION {x.Item1}", !x.Item2));
    }
  }

  public class Not : BuildingRequirement {
    private BuildingRequirement requirement;
    public Not(BuildingRequirement requirement) {
      this.requirement = requirement;
    }

    public override bool IsMet(BuildingPlot plot) {
      return !requirement.IsMet(plot);
    }

    public override IEnumerable<(string, bool)> Requirements(BuildingPlot plot)
    {
      return requirement.Requirements(plot).Select(x => ($"NOT {x.Item1}", !x.Item2));
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

    public override IEnumerable<(string, bool)> Requirements(BuildingPlot plot)
    {
      return requirements.Select(x => x.Requirements(plot)).Aggregate((x, y) => x.Concat(y));
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

    public override IEnumerable<(string, bool)> Requirements(BuildingPlot plot)
    {
      int powerConsumption = power - (plot.Building?.PowerConsumption ?? 0);
      if (powerConsumption > 0) {
        yield return ($"Required Power: {powerConsumption}", IsMet(plot));
      } else {
        yield break;
      }
    }
  }

  public class BuildingCost : BuildingRequirement {
    private (Good good, int amount)[] costs;
    public BuildingCost((Good, int)[] costs) {
      this.costs = costs ?? new (Good, int)[] {};
    }

    public override bool IsMet(BuildingPlot plot) {
      foreach ((Good good, int amount) in costs) {
        if (Colony.Instance.GetStockpile(good) < amount) {
          return false;
        }
      }
      return true;
    }

    public override IEnumerable<(string, bool)> Requirements(BuildingPlot plot)
    {
      foreach ((Good good, int amount) in costs) {
        yield return ($"{good.Name}: {amount}", Colony.Instance.GetStockpile(good) >= amount);
      }
    }
  }

  private class FunctionalRequirement : BuildingRequirement {
    public delegate bool RequirementFunction(BuildingPlot plot);
    private RequirementFunction func;
    private string desc;
    public FunctionalRequirement(RequirementFunction func, string desc) {
      this.func = func;
      this.desc = desc;
    }
    public override bool IsMet(BuildingPlot plot) => func(plot);
    public override IEnumerable<(string, bool)> Requirements(BuildingPlot plot) {
      if (desc == null) {
        yield break;
      } else {
        yield return (desc, IsMet(plot));
      }
    }
  }
}

