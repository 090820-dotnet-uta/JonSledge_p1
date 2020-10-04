﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using p1_2.Data;
using p1_2.Models;
using p1_2.Utils;
using Microsoft.Extensions.Caching.Memory;
using p1_2.DbManip;

namespace p1_2.Controllers
{
  public class ProductController : Controller
  {
    private readonly BookopolisDbContext _db;
    private IMemoryCache _cache;
    List<ShoppingCart> shoppingCartProducts;
    public ProductController(BookopolisDbContext db, IMemoryCache cache)
    {
      _cache = cache;
      _db = db;

      if (!_cache.TryGetValue("shoppingCart", out shoppingCartProducts))
      {
        _cache.Set("shoppingCart", new List<ShoppingCart>());
        _cache.TryGetValue("shoppingCart", out shoppingCartProducts);
      }
    }
    public IActionResult Index(int? id)
    {
      if (Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }

      List<ShoppingCart> shoppingCartProds = (List<ShoppingCart>)_cache.Get("shoppingCart");

      _cache.Set("StoreId", id);
      Store store = DbManipulation.GetStore(_db, id);
      _cache.Set("Store", store);

      List<Inventory> inventories = DbManipulation.GetInventoriesOfStore(_db, id).OrderBy(pid => pid.ProductId).ToList();
      List<Product> prodList = DbManipulation.GetProducts(_db).ToList();

      DbManipulation.SetShoppingCartStock(shoppingCartProds, inventories);
      IEnumerable<ProductView> EProdViews = DbManipulation.CreateProductViews(prodList, inventories);


      return View(EProdViews);
    }

    public IActionResult Details(int? id)
    {
      if (Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }
      if (id == null)
      {
        return NotFound();
      }

      Inventory inv = DbManipulation.GetInventoryOfStoreProduct(_db, (int)_cache.Get("StoreId"), id);
      Product prod = DbManipulation.GetProduct(_db, id);

      if (prod == null)
      {
        return NotFound();
      }

      List<ShoppingCart> shoppingCartInvs = DbManipulation.GetInventoryOfShoppingCart(shoppingCartProducts, (int)_cache.Get("StoreId"), id);

      ProductView productView = DbManipulation.CreateProductView(shoppingCartInvs, inv, prod);


      return View(productView);
    }

    public IActionResult AddToCart(ProductView p)
    {
      int temp = (int)_cache.Get("StoreId");
      Store store = (Store)_cache.Get("Store");
      p.Amount--;
      ShoppingCart sh = new ShoppingCart()
      {
        StockAmount = p.Amount,
        Author = p.Author,
        Title = p.Title,
        Price = p.Price,
        StoreId = temp,
        ProductId = p.ProductId,
        State = store.State
      };
      var inv = _db.Inventories.FirstOrDefault(i => i.ProductId == sh.ProductId && i.StoreId == (int)_cache.Get("StoreId"));

      sh.Inventory = inv;
      shoppingCartProducts.Add(sh);

      _cache.Set("shoppingCart", shoppingCartProducts);

      return RedirectToAction("Index", "Store");
    }

  }
}
