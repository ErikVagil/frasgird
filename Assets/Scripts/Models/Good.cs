/// <summary>
/// Represents a given trade good, consumed, produced or traded.
/// Right now it's just a name, but we can put information like
/// base price here if we want.
/// </summary>
public class Good {
  public string Name { get; private set; }
  public Good(string name) {
    Name = name;
  }
}