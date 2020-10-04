using System;
using Xunit;
using p1_2.Controllers;
using Microsoft.EntityFrameworkCore;
using p1_2.Data;
using Microsoft.AspNetCore.Mvc;
using p1_2.Models;
using p1_2.Utils;
using System.Collections.Generic;
using p1_2.DbManip;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace p1_2.Test
{

  public class UnitTest1
  {
    private HomeController _homeController;
    public UnitTest1()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
      .UseInMemoryDatabase(databaseName: "HomeIndexViewIsReturned")
      .Options;

      using (var context = new BookopolisDbContext(options))
      {
        _homeController = new HomeController(context);
      }

    }

    [Fact]
    public void HomeIndexViewIsReturned()
    {
      var result = _homeController.Privacy() as ViewResult;
      Console.WriteLine(result);
      Assert.Equal("Privacy", result.ViewName);
    }

    [Fact]
    public void IsLoggedInReturnsCorrectly()
    {
      var services = new ServiceCollection();
      services.AddMemoryCache();
      var serviceProvider = services.BuildServiceProvider();
      var memoryCache = serviceProvider.GetService<IMemoryCache>();

      Assert.False(Util.IsLoggedIn(memoryCache));

      memoryCache.Set("UserName", "bruh");

      Assert.True(Util.IsLoggedIn(memoryCache));
    }

    [Fact]
    public void GetShoppingCartStoreProductsReturnsCorrectStoreProducts()
    {
      ShoppingCart shoppingCartToCheck = new ShoppingCart()
      {
        ShoppingCartId = 1,
        Inventory = new Inventory() { InventoryId = 1 },
        ProductId = 1,
        StoreId = 1,
        StockAmount = 1
      };

      List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>()
      {
        shoppingCartToCheck,
        new ShoppingCart()
        {
          ShoppingCartId = 2,
          Inventory = new Inventory() { InventoryId = 2 },
          ProductId = 1,
          StoreId = 1,
          StockAmount = 0
        },
        new ShoppingCart()
        {
          ShoppingCartId = 3,
          Inventory = new Inventory() { InventoryId = 3 },
          ProductId = 1,
          StoreId = 2,
          StockAmount = 2
        }
      };
      List<ShoppingCart> result = Util.GetShoppingCartStoreProducts(shoppingCartProducts, shoppingCartToCheck);

      Assert.Equal(2, result.Count);
      foreach (ShoppingCart sh in result)
      {
        Assert.Equal(1, sh.ProductId);
        Assert.Equal(1, sh.StoreId);
      }
    }

    [Fact]
    public void FindAndRemoveShoppingCartRemovesCorrectShoppingCart()
    {
      ShoppingCart shoppingCartToRemove = new ShoppingCart()
      {
        ShoppingCartId = 1,
        Inventory = new Inventory() { InventoryId = 1 },
        ProductId = 1,
        StoreId = 1,
        StockAmount = 0
      };

      List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>()
      {
        shoppingCartToRemove,
        new ShoppingCart()
        {
          ShoppingCartId = 2,
          Inventory = new Inventory() { InventoryId = 1 },
          ProductId = 1,
          StoreId = 1,
          StockAmount = 1
        },
        new ShoppingCart()
        {
          ShoppingCartId = 3,
          Inventory = new Inventory() { InventoryId = 2 },
          ProductId = 1,
          StoreId = 2,
          StockAmount = 2
        }
      };
      List<ShoppingCart> resultShopList = Util.GetShoppingCartStoreProducts(shoppingCartProducts, shoppingCartToRemove);

      Assert.Equal(2, resultShopList.Count);

      ShoppingCart resultShop = Util.FindAndRemoveShoppingCart(resultShopList);

      Assert.Equal(shoppingCartToRemove, resultShop);
    }

    [Fact]
    public void IsSeededReturnsFalseWhenEmpty()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "IsSeededReturnsFalseWhenEmpty")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        Assert.False(Util.IsSeeded(context));
      }
    }

    [Fact]
    public void IsSeededReturnsTrueWhenSeeded()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "IsSeededReturnsTrueWhenSeeded")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);
        Assert.True(Util.IsSeeded(context));
      }
    }

    [Fact]
    public void SeedDbAddsAllData()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "SeedDbAddsAllData")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);
        SeedData seedData = new SeedData();

        Assert.Equal(seedData.Products.Count, context.Products.Count());
        Assert.Equal(seedData.Stores.Count, context.Stores.Count());
      }
    }

    [Fact]
    public void IsStoreIsInRange()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "IsStoreIsInRange")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        Assert.InRange(4, 1, 5);
        Assert.False(DbManipulation.IsStore(context, 5));
        Assert.True(DbManipulation.IsStore(context, 4));
      }
    }

    [Fact]
    public void GetStoresReturnsAllStores()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetStoresReturnsAllStores")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        IEnumerable<Store> stores = DbManipulation.GetStores(context);
        List<Store> storesList = stores.ToList();
        Assert.InRange(storesList.Count, 1, storesList.Count + 1);
      }
    }

    [Fact]
    public void GetStoreReturnsCorrectStore()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetStoreReturnsCorrectStore")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        Store store = DbManipulation.GetStore(context, 2);
        Assert.IsType<Store>(store);
        Assert.Equal(2, store.StoreId);
        Assert.NotEqual(3, store.StoreId);
      }
    }

    [Fact]
    public void GetInventoriesOfStoreReturnsCorrectInventories()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetInventoriesOfStoreReturnsCorrectInventories")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        IEnumerable<Inventory> inventories = DbManipulation.GetInventoriesOfStore(context, 3);
        int count = inventories.ToList().Select(inv => inv.StoreId).Count();

        Assert.Equal(context.Products.Count(), count);
      }
    }

    [Fact]
    public void GetInventoryOfStoreProductReturnsCorrectInventory()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetInventoryOfStoreProductReturnsCorrectInventory")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        Inventory inventory1 = DbManipulation.GetInventoryOfStoreProduct(context, 4, 2);
        Inventory expectedInv1 = context.Inventories.FirstOrDefault(inv => inv.StoreId == 4 && inv.ProductId == 2);

        Inventory inventory2 = DbManipulation.GetInventoryOfStoreProduct(context, 4, 2);
        Inventory expectedInv2 = context.Inventories.FirstOrDefault(inv => inv.StoreId == 3 && inv.ProductId == 2);

        Assert.Equal(expectedInv1, inventory1);
        Assert.NotEqual(expectedInv2, inventory2);
      }
    }

    [Fact]
    public void GetProductsReturnsAllProducts()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetProductsReturnsAllProducts")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        IEnumerable<Product> products = DbManipulation.GetProducts(context);
        List<Product> productsList = products.ToList();
        Assert.InRange(productsList.Count, 1, productsList.Count + 1);
      }
    }

    [Fact]
    public void GetProductReturnsCorrectProduct()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetProductReturnsCorrectProduct")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        Product product = DbManipulation.GetProduct(context, 7);
        Assert.IsType<Product>(product);
        Assert.Equal(7, product.ProductId);
        Assert.NotEqual(3, product.ProductId);
      }
    }

    [Fact]
    public void SetShoppingCartStockSetsCorrectStock()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "SetShoppingCartStockSetsCorrectStock")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>()
        {
          new ShoppingCart()
          {
            Inventory = new Inventory() { InventoryId = 1 },
            StockAmount = 0
          },
          new ShoppingCart()
          {
            Inventory = new Inventory() { InventoryId = 2 },
            StockAmount = 1
          }
        };

        List<Inventory> inventories = DbManipulation.GetInventoriesOfStore(context, 1).OrderBy(pid => pid.ProductId).ToList();
        DbManipulation.SetShoppingCartStock(shoppingCartProducts, inventories);
        Inventory correctStockAmount1 = inventories.FirstOrDefault(inv => inv.InventoryId == 1);
        Inventory correctStockAmount2 = inventories.FirstOrDefault(inv => inv.InventoryId == 2);

        Assert.Equal(0, correctStockAmount1.Amount);
        Assert.Equal(1, correctStockAmount2.Amount);
      }
    }

    [Fact]
    public void CreateProductViewsHasCorrectInventoryAmount()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "CreateProductViewsHasCorrectInventoryAmount")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        List<Product> products = DbManipulation.GetProducts(context).ToList();
        List<Inventory> inventories = DbManipulation.GetInventoriesOfStore(context, 1).ToList();

        foreach (Inventory inventory in inventories)
        {
          inventory.Amount = 1;
        }

        List<ProductView> productViews = DbManipulation.CreateProductViews(products, inventories).ToList();

        foreach (ProductView productView in productViews)
        {
          Assert.Equal(1, productView.Amount);
        }
      }
    }

    [Fact]
    public void GetInventoriesReturnsAllInventories()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetInventoriesReturnsAllInventories")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        IEnumerable<Inventory> inventories = DbManipulation.GetInventories(context);
        List<Inventory> inventoriesList = inventories.ToList();
        Assert.InRange(inventoriesList.Count, 1, inventoriesList.Count + 1);
      }
    }

    [Fact]
    public void GetInventoryOfShoppingCartReturnsCorrectShoppingCart()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "GetInventoryOfShoppingCartReturnsCorrectShoppingCart")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        List<Product> products = DbManipulation.GetProducts(context).ToList();
        List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>()
        {
          new ShoppingCart()
          {
            Inventory = new Inventory() { InventoryId = 1 },
            ProductId = 1,
            StoreId = 1,
            StockAmount = 1
          },
          new ShoppingCart()
          {
            Inventory = new Inventory() { InventoryId = 2 },
            ProductId = 1,
            StoreId = 1,
            StockAmount = 0
          },
          new ShoppingCart()
          {
            Inventory = new Inventory() { InventoryId = 3 },
            ProductId = 1,
            StoreId = 2,
            StockAmount = 2
          }
        };

        List<ShoppingCart> result = DbManipulation.GetInventoryOfShoppingCart(shoppingCartProducts, 1, 1);

        foreach (ShoppingCart shoppingCart in result)
        {
          Assert.Equal(1, shoppingCart.StoreId);
          Assert.Equal(1, shoppingCart.ProductId);
        }
      }
    }

    [Fact]
    public void CreateProductViewHasCorrectInventoryAmount()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "CreateProductViewHasCorrectInventoryAmount")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        Inventory inv = DbManipulation.GetInventoryOfStoreProduct(context, 1, 3);
        Product prod = DbManipulation.GetProduct(context, 3);

        List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>()
        {
          new ShoppingCart()
          {
            ShoppingCartId = 1,
            StoreId = 1,
            ProductId = 3,
            StockAmount = 2
          },
          new ShoppingCart()
          {
            ShoppingCartId = 2,
            StoreId = 1,
            ProductId = 3,
            StockAmount = 1
          },
          new ShoppingCart()
          {
            ShoppingCartId = 3,
            StoreId = 1,
            ProductId = 3,
            StockAmount = 0
          },
        };

        List<ShoppingCart> shoppingCartInvs = DbManipulation.GetInventoryOfShoppingCart(shoppingCartProducts, 1, 3);

        ProductView productView = DbManipulation.CreateProductView(shoppingCartInvs, inv, prod);

        Assert.Equal(0, productView.Amount);
      }
    }

    [Fact]
    public void CreateShoppingCartHasCorrectInventoryAmount()
    {
      var options = new DbContextOptionsBuilder<BookopolisDbContext>()
        .UseInMemoryDatabase(databaseName: "CreateShoppingCartHasCorrectInventoryAmount")
        .Options;

      using (var context = new BookopolisDbContext(options))
      {
        DbManipulation.SeedDb(context);

        ProductView productView = new ProductView()
        {
          ProductViewId = 1,
          Title = "Catch-22",
          Author = "Joseph Heller",
          Description = "Catch-22 is a satirical war novel by American author Joseph Heller.",
          Price = 12.50,
          Amount = 2,
          ProductId = 1,
          IsInCart = true,
        };

        ShoppingCart shoppingCart = DbManipulation.CreateShoppingCart(productView, 1, "Missouri");

        Assert.Equal(1, shoppingCart.StockAmount);
        Assert.Equal("Missouri", shoppingCart.State);
      }
    }

  }
}
