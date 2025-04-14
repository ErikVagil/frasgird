using System.Collections.Generic;
public static class Goods
{
  private static List<Good> allGoods = new();
  public static IEnumerable<Good> All { get => allGoods; }
  public static Good GetGoodFromID(int id)
  {
    return allGoods[id];
  }

  /// <summary>
  /// Use this function when declaring new goods to make sure
  /// they get added to the list.
  /// </summary>
  /// <param name="good"></param>
  /// <returns></returns>
  private static Good RegisterGood(Good good)
  {
    allGoods.Add(good);
    return good;
  }
  public static Good Food = RegisterGood(new Good("Food"));
  public static Good Water = RegisterGood(new Good("Water"));
  // public static Good Silichips = RegisterGood(new Good("Silichips"));

  /// <summary>
  /// Represents building materials right now.
  /// </summary>
  public static Good Steel = RegisterGood(new Good("Steel"));
}