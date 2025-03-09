public class BuildingPlot {
  public int Ring { get; private set; }
  public int PlotNumber { get; private set; }
  public Building Building { get; private set; }
  public delegate void OnBuild(Building building);
  private OnBuild onBuild;
  public BuildingPlot(int ring, int plotNumber) {
    Ring = ring;
    PlotNumber = plotNumber;
  }
  public void Build(Building building) {
    Building = building;
    onBuild?.Invoke(building);
  }
  public void Demolish() {
    Building = null;
    onBuild?.Invoke(null);
  }
  public void SubscribeToOnBuild(OnBuild onBuild) {
    this.onBuild += onBuild;
  }
  public void UnsubscribeToOnBuild(OnBuild onBuild) {
    this.onBuild -= onBuild;
  }
}