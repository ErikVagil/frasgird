public class BuildingPlot {
  public int Ring { get; private set; }
  public int PlotNumber { get; private set; }
  public BuildingPlot(int ring, int plotNumber) {
    Ring = ring;
    PlotNumber = plotNumber;
  }
}