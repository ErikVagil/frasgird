public class Building {
  public string Name { get; private set; }
  public int PowerConsumption { get; private set; }
  public Building(string name, int powerConsumption) {
    Name = name;
    PowerConsumption = powerConsumption;
  }
}


