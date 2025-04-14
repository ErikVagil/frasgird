/// <summary>
/// Represents the game logic contained in a building plot, a 1x2 tile
/// within the colony. Created as an aggregation within ColonyMap.cs.
/// </summary>
public class BuildingPlot {
  /// <summary>
  /// Which ring of buildings is this a part of?
  /// </summary>
  public int Ring { get; private set; }

  /// <summary>
  /// ID for the plot's location within its ring.
  /// </summary>
  public int PlotNumber { get; private set; }

  /// <summary>
  /// What is the current building that is built? Should never be null;
  /// instead, represent no building with Buildings.Empty.
  /// </summary>
  public Building Building { get; private set; }

  /// <summary>
  /// Delegate type referring to OnBuild Events
  /// </summary>
  /// <param name="building">The building that is built.</param>
  public delegate void OnBuild(Building building);

  /// <summary>
  /// Event handler for when a building is built.
  /// </summary>
  private OnBuild onBuild;
  public BuildingPlot(int ring, int plotNumber) {
    Ring = ring;
    PlotNumber = plotNumber;
    Build(Buildings.Empty);
  }

  /// <summary>
  /// Sets the current building to the new one, and calls the event
  /// handler.
  /// </summary>
  /// <param name="building">The building that was built.</param>
  public void Build(Building building) {
    Building = building;
    onBuild?.Invoke(building);
  }

  /*** Event handler subscription methods ***/

  public void SubscribeToOnBuild(OnBuild onBuild) {
    this.onBuild += onBuild;
  }
  public void UnsubscribeToOnBuild(OnBuild onBuild) {
    this.onBuild -= onBuild;
  }
}