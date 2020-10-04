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

    public static IEnumerable<ProductView> CreateProductViews(List<Product> prodList, List<Inventory> inventories)
    {
      List<ProductView> prodViews = new List<ProductView>();
      for (int i = 0; i < inventories.Count; i++)
      {
        ProductView productView = new ProductView()
        {
          Amount = inventories[i].Amount,
          Author = prodList[i].Author,
          Description = prodList[i].Description,
          Price = prodList[i].Price,
          ProductId = prodList[i].ProductId,
          Title = prodList[i].Title
        };
        prodViews.Add(productView);
      }
      IEnumerable<ProductView> EProdViews = prodViews;
      return EProdViews;
    }

    public static IEnumerable<Inventory> GetInventories(BookopolisDbContext context)
      => context.Inventories;

    public static List<ShoppingCart> GetInventoryOfShoppingCart(List<ShoppingCart> shoppingCartProducts, int? storeId, int? productId)
      => shoppingCartProducts.Where(sh => sh.StoreId == storeId && sh.ProductId == productId).ToList();

    public static ProductView CreateProductView(List<ShoppingCart> shoppingCartInvs, Inventory inv, Product prod)
    {
      ProductView productView = new ProductView();

      productView.Amount = shoppingCartInvs.Count > 0 ? shoppingCartInvs[shoppingCartInvs.Count - 1].StockAmount : inv.Amount;
      productView.Author = prod.Author;
      productView.Description = prod.Description;
      productView.Price = prod.Price;
      productView.ProductId = prod.ProductId;
      productView.Title = prod.Title;
      productView.IsInCart = (shoppingCartInvs.Count() == 2);

      return productView;
    }

    public static ShoppingCart CreateShoppingCart(ProductView productView, int storeId, string state)
    {
      productView.Amount--;
      ShoppingCart sh = new ShoppingCart()
      {
        StockAmount = productView.Amount,
        Author = productView.Author,
        Title = productView.Title,
        Price = productView.Price,
        StoreId = storeId,
        ProductId = productView.ProductId,
        State = state
      };

      return sh;
    }

  }
}
