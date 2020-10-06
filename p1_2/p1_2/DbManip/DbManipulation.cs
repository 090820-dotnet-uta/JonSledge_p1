using p1_2.Data;
using p1_2.Models;
using System.Collections.Generic;
using System.Linq;

namespace p1_2.DbManip
{
  public class DbManipulation
  {

    public static void SeedDb(BookopolisDbContext context)
    {
      SeedData seedData = new SeedData();

      foreach (var prod in seedData.Products) context.Products.Add(prod);
      context.SaveChanges();

      var prodContext = context.Products.ToList();

      foreach (var store in seedData.Stores)
      {
        List<Inventory> inventories = new List<Inventory>();
        foreach (var prod in prodContext)
        {
          Inventory inventory = new Inventory();
          inventory.ProductId = prod.ProductId;
          inventory.Amount = 2;
          inventories.Add(inventory);
        }
        store.Inventories = inventories;
      }

      foreach (var store in seedData.Stores) context.Stores.Add(store);
      context.SaveChanges();
    }

    public static bool IsSeeded(BookopolisDbContext _db)
      => (_db.Products.Count() > 0 && _db.Stores.Count() > 0);

    public static bool IsStore(BookopolisDbContext context, int? id)
      => (context.Stores.FirstOrDefault(s => s.StoreId == id) != null);

    public static IEnumerable<Store> GetStores(BookopolisDbContext context)
      => context.Stores;

    public static Store GetStore(BookopolisDbContext context, int? id)
      => context.Stores.FirstOrDefault(s => s.StoreId == id);

    public static IEnumerable<Inventory> GetInventoriesOfStore(BookopolisDbContext context, int? id)
      => context.Inventories.Where(i => i.StoreId == id);

    public static Inventory GetInventoryOfStoreProduct(BookopolisDbContext context, int? storeId, int? productId)
      => context.Inventories.FirstOrDefault(i => i.ProductId == productId && i.StoreId == storeId);

    public static IEnumerable<Product> GetProducts(BookopolisDbContext context)
      => context.Products;

    public static Product GetProduct(BookopolisDbContext context, int? id)
      => context.Products.FirstOrDefault(p => p.ProductId == id);

    public static void SetShoppingCartStock(List<ShoppingCart> shoppingCartProds, List<Inventory> inventories)
    {
      for (int i = 0; i < shoppingCartProds.Count; i++)
      {
        for (int j = 0; j < inventories.Count; j++)
        {
          if (shoppingCartProds[i].Inventory.InventoryId == inventories[j].InventoryId)
          {
            inventories[j].Amount = shoppingCartProds[i].StockAmount;
          }
        }
      }
    }

    public static IEnumerable<Inventory> GetInventories(BookopolisDbContext context)
      => context.Inventories;

    public static Customer GetCustomer(BookopolisDbContext context, int? customerId)
      => context.Customers.FirstOrDefault(c => c.CustomerId == customerId);

    public static IEnumerable<CustomerAddress> GetCustomerAddresses(BookopolisDbContext context, int? customerId)
       => context.CustomerAddresses.Where(ca => ca.CustomerId == customerId);

    public static void UpdateInventoryAmounts(BookopolisDbContext context, Dictionary<int, int> myDict)
    {
      for (int i = 0; i < myDict.Keys.Count; i++)
      {
        for (int j = 0; j < context.Inventories.ToList().Count; j++)
        {
          if (myDict.Keys.ToList()[i] == context.Inventories.ToList()[j].InventoryId)
          {
            context.Inventories.ToList()[j].Amount -= myDict[context.Inventories.ToList()[j].InventoryId];
          }
        }
      }
    }

    public static List<ShoppingCart> GetInventoryOfShoppingCart(List<ShoppingCart> shoppingCartProducts, int? storeId, int? productId)
      => shoppingCartProducts.Where(sh => sh.StoreId == storeId && sh.ProductId == productId).ToList();

  }
}
