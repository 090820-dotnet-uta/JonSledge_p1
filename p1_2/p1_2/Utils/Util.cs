using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using p1_2.Data;
using p1_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace p1_2.Utils
{
  public static class Util
  {
    public static bool IsLoggedIn(IMemoryCache cache)
    {
      return (cache.Get("UserName") != null);
    }

    public static bool IsSeeded(BookopolisDbContext _db)
    {
      return (_db.Products.Count() > 0 && _db.Stores.Count() > 0);
    }

    public static List<ShoppingCart> GetShoppingCartStoreProducts(List<ShoppingCart> shoppingCartProducts, ShoppingCart sh)
      => shoppingCartProducts.Where(s => s.ProductId == sh.ProductId && s.StoreId == sh.StoreId).ToList();

    public static ShoppingCart FindAndRemoveShoppingCart(List<ShoppingCart> shops)
      => shops.Find(s => s.StockAmount == shops.Min(s => s.StockAmount));


    public static void Shuffle(List<string> l, int n)
    {
      Random r = new Random();

      for (int i = n - 1; i > 1; i--)
      {
        int idx = r.Next(i + 1);
        string val = l[idx];
        l[idx] = l[i];
        l[i] = val;
      }
    }
  }
}
