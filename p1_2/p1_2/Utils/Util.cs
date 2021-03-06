﻿using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using p1_2.Models;

namespace p1_2.Utils
{
  public static class Util
  {
    public static bool IsLoggedIn(IMemoryCache cache)
    {
      return (cache.Get("UserName") != null);
    }

    public static List<ShoppingCart> GetShoppingCartStoreProducts(List<ShoppingCart> shoppingCartProducts, ShoppingCart sh)
      => shoppingCartProducts.Where(s => s.ProductId == sh.ProductId && s.StoreId == sh.StoreId).ToList();

    public static ShoppingCart FindAndRemoveShoppingCart(List<ShoppingCart> shops)
      => shops.Find(s => s.StockAmount == shops.Min(s => s.StockAmount));

    public static CustomerAddress CheckForRepeatAddress(List<CustomerAddress> customerAddresses, CustomerAddress customerAddress)
    {
      foreach (CustomerAddress ca in customerAddresses)
      {
        if (ca.City == customerAddress.City
            && ca.State == customerAddress.State
            && ca.StreetAddress == customerAddress.StreetAddress
            && ca.ZIP == customerAddress.ZIP)
        {
          customerAddress = ca;
        }
      }
      return customerAddress;
    }

    public static Dictionary<int, int> GetShoppingCartInventoryAmounts(List<ShoppingCart> shoppingCart)
    {
      Dictionary<int, int> myDict = new Dictionary<int, int>();

      for (int i = 0; i < shoppingCart.Count; i++)
      {
        myDict[shoppingCart[i].Inventory.InventoryId] = shoppingCart.Count(s => s.Inventory.InventoryId == shoppingCart[i].Inventory.InventoryId);
      }

      return myDict;
    }

    public static List<OrderProduct> CreateOrderProducts(List<ShoppingCart> shoppingCart, Customer customer, CustomerAddress customerAddress)
    {
      List<OrderProduct> orderProducts = new List<OrderProduct>();
      foreach (var sh in shoppingCart)
      {
        OrderProduct orderProduct = new OrderProduct();
        orderProduct.ProductId = sh.ProductId;
        orderProduct.StoreId = sh.StoreId;
        orderProduct.CustomerId = customer.CustomerId;
        orderProduct.CustomerAddress = customerAddress;
        orderProducts.Add(orderProduct);
      }
      return orderProducts;
    }

  }
}
