using System.Collections.Generic;
public static class Goods {
  private static List<Good> allGoods = new ();
  public static IEnumerable<Good> All { get => allGoods; }
  public static Good GetGoodFromID(int id) {
    return allGoods[id];
  }
  private static Good RegisterGood(Good good) {
    allGoods.Add(good);
    return good;
  }
  public static Good Food = RegisterGood(new Good("Food"));
  public static Good Water = RegisterGood(new Good("Water"));
  // public static Good Silichips = RegisterGood(new Good("Silichips"));
  public static Good Steel = RegisterGood(new Good("Steel"));
}