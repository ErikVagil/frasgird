using System.Collections.Generic;
using System;

/// <summary>
/// Created as a subcomponent of Colony to manage the BuildingPlot
/// objects.
/// </summary>
public class ColonyMap {
  private List<List<BuildingPlot>> plots;
  public int NumRings => plots.Count;
  public int NumPlots(int ring) => plots[ring].Count;
  public IEnumerable<BuildingPlot> AllPlots { get {
    foreach (var ring in plots) {
      foreach (var plot in ring) {
        yield return plot;
      }
    }
  }}
  public ColonyMap() {
    plots = new ();
    for(int i = 0; i < 5; i++) {
      AddRing();
    }
  }
  public BuildingPlot GetPlot(int ring, int number) {
    if (ring >= NumRings || number >= NumPlots(ring)) {
      throw new Exception("Plot doesn't exist.");
    } else {
      return plots[ring][number];
    }
  }
  private void AddRing() {
    int numPlots;
    if (NumRings == 0) {
      numPlots = 2;
    } else {
      numPlots = plots[NumRings - 1].Count + 4;
    }
    plots.Add(new ());
    for (int i = 0; i < numPlots; i++) {
      plots[NumRings - 1].Add(new BuildingPlot(NumRings - 1, i));
    }
  }
}