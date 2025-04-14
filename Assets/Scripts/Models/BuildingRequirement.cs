using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class may be a bit confusing; it's used to determine whether
/// a building can be built. There's some nested logic here to allow more
/// in-depth building requirements.
/// </summary>
public abstract class BuildingRequirement {
  /// <summary>
  /// Returns whether the BuildingRequirement is met for
  // the given BuildingPlot. Often uses Colony.Instance as well.
  /// </summary>
  /// <param name="plot"></param>
  /// <returns></returns>
  public abstract bool IsMet(BuildingPlot plot);

  /// <summary>
  /// Returns an Enumerable of tuples. The first item explains
  /// a BuildingRequirement subitem, and the latter explains whether that
  /// subitem is met.
  /// </summary>
  /// <param name="plot"></param>
  /// <returns></returns>
  public abstract IEnumerable<(string, bool)> Requirements(BuildingPlot plot);

  /// <summary>
  /// Default building requirement that is always met.
  /// </summary>
  public static BuildingRequirement None { get; private set; } = new FunctionalRequirement((BuildingPlot plot) => true, null);

  /// <summary>
  /// IsEmpty is met so long as the BuildingPlot is "Empty Plot".
  /// </summary>
  public static BuildingRequirement IsEmpty { get; private set; } = new BaseBuilding("Empty Plot");

  /// <summary>
  /// Requires every ring up until this ring to be filled with
  /// buildings.
  /// </summary>
  public static BuildingRequirement FilledRingRequirement { get; private set;} = new FunctionalRequirement(
    (BuildingPlot plot) => {
    for (int i = 0; i < plot.Ring; i++) {
        if (!Colony.Instance.Map.isRingFilled(i)) {
          return false;
        }
      }
      return true;
  }, "Outer Ring");

  /// <summary>
  /// BuildingRequirement that is met so long as the current
  /// building in the plot is the provided base building required.
  /// </summary>
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

  /// <summary>
  /// This requirement is satisfied so long as at least one
  /// of its subrequirements is satisfied, like an OR statement.
  /// </summary>
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
      return requirements
              .Take(1)
              .Select(x => x.Requirements(plot))
              .Aggregate((x, y) => x.Concat(y))
              .Select(x => ($"\t{x.Item1}", x.Item2))
              .Concat(requirements.Skip(1)
                      .Select(x => x.Requirements(plot))
                      .Aggregate((x, y) => x.Concat(y))
                      .Select(x => ($"OR\t{x.Item1}", x.Item2)));
    }
  }

  /// <summary>
  /// This requirement is satisfied so long as the subrequirement is
  /// not satisfied. This allows you to detect if a building hasn't already
  /// been built, for example.
  /// </summary>
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

  /// <summary>
  /// This requirement is met so long as all subrequirements are
  /// met. Think of it as an AND statement.
  /// </summary>
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

  /// <summary>
  /// This requirement is met so long as the construction of the new
  /// building wouldn't result in a negative power supply. Pass the
  /// required power in; it will do the necessary calculations to factor
  /// in the power gained by replacing the old building.
  /// </summary>
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

  /// <summary>
  /// This requirement is met so long as each good/amount tuple
  /// in the provided buliding costs array is in stock in the colony.
  /// </summary>
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

  /// <summary>
  /// This requirement allows you to make a custom requirement
  /// without writing an entire subclass. Expose it as a member. Used
  /// by None, and the older version of IsEmpty.
  /// </summary>
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

