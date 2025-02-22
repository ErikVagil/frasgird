using System.Collections.Generic;
public static class Resources {
  private static List<Resource> allResources = new ();
  public static IEnumerable<Resource> All { get => allResources; }
  public static Resource Food = new Resource("Food");
  public static Resource Water = new Resource("Water");
}