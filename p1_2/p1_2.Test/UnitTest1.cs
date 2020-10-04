using System;
using Xunit;
using p1_2.Controllers;
using Microsoft.EntityFrameworkCore;
using p1_2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using p1_2.Models;
using p1_2.Utils;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using p1_2.DbManip;
using System.Linq;

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

  }
}
