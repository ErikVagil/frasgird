using System.Collections.Generic;
public static class Goods {
  private static List<Good> allGoods = new ();
  public static IEnumerable<Good> All { get => allGoods; }
  public static Good Food = new Good("Food");
  public static Good Water = new Good("Water");
}