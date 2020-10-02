using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using p1_2.Data;
using p1_2.Models;

namespace p1_2.Controllers
{
  public class OrderController : Controller
  {
    private IMemoryCache _cache;//must set this for DI in Startup.cs
    private readonly BookopolisDbContext _db;
    List<ShoppingCart> shoppingCart;
    public OrderController(IMemoryCache cache, BookopolisDbContext db)
    {
      _cache = cache;
      _db = db;

      if (!_cache.TryGetValue("shoppingCart", out shoppingCart))
      {
        _cache.Set("shoppingCart", new List<ShoppingCart>());
        _cache.TryGetValue("shoppingCart", out shoppingCart);
      }
    }

    public IActionResult SelectStore(int? id)
    {
      ViewData["StoreOrders"] = "active";
      var orderProducts = _db.OrderProducts.Where(op => op.StoreId == id).Distinct();
      var stores = _db.Stores.ToList();

      if (id == 0)
      {
        ViewData["CurrentStore"] = "none";
      }
      else
      {
        ViewData["CurrentStore"] = stores.FirstOrDefault(s => s.StoreId == id).State;
      }

      if (orderProducts.ToList().Count == 0)
      {
        OrderView orderViewNone = new OrderView();
        orderViewNone.EmptyMessage = "There are no orders for that state";
        orderViewNone.IsEmpty = true;
        orderViewNone.Stores = stores;
        return View("StoreOrders", orderViewNone);
      }

      var inboth = (from p1 in _db.Orders
                    join p2 in orderProducts
                    on p1.OrderId equals p2.OrderId
                    select p1).Distinct();
      var res = inboth.ToList();

      OrderView orderView = new OrderView();
      orderView.Orders = res;
      orderView.Stores = stores;


      //return RedirectToAction("StoreOrders", new { id });
      return View("StoreOrders", orderView);

    }

    public IActionResult StoreOrders(int id)
    {
      ViewData["StoreOrders"] = "active";
      var orderProducts = _db.OrderProducts.Where(op => op.StoreId == id).Distinct();
      var stores = _db.Stores.ToList();
      if (id == 0)
      {
        ViewData["CurrentStore"] = "none";
      }
      else
      {
        ViewData["CurrentStore"] = stores.FirstOrDefault(s => s.StoreId == id).State;
      }

      if (orderProducts.ToList().Count == 0)
      {
        OrderView orderViewNone = new OrderView();
        orderViewNone.EmptyMessage = "There are no orders for that state";
        orderViewNone.IsEmpty = true;
        orderViewNone.Stores = stores;
        return View(orderViewNone);
      }

      var orders = _db.Orders.Where(o => orderProducts.All(op => op.OrderId == o.OrderId)).ToList();


      OrderView orderView = new OrderView();
      orderView.Orders = orders;
      orderView.Stores = stores;


      return View(orderView);
    }

    public IActionResult MyOrders()
    {
      ViewData["MyOrders"] = "active";
      Customer tempCust = (Customer)_cache.Get("LoggedInCustomer");
      var orderProducts = _db.OrderProducts.Where(op => op.CustomerId == tempCust.CustomerId);

      var stores = _db.Stores.ToList();

      var inboth = (from p1 in _db.Orders
                    join p2 in orderProducts
                    on p1.OrderId equals p2.OrderId
                    select p1).Distinct();
      var res = inboth.ToList();


      OrderView orderView = new OrderView();
      orderView.Orders = res;
      orderView.Stores = stores;
      return View(orderView);
    }

    public IActionResult SelectCustomer(int? id)
    {
      ViewData["CustomerOrders"] = "active";
      var orderProducts = _db.OrderProducts.Where(op => op.CustomerId == id).Distinct();
      var customers = _db.Customers.ToList();

      if (id == 0)
      {
        ViewData["CurrentCustomer"] = "none";
      }
      else
      {
        ViewData["CurrentCustomer"] = customers.FirstOrDefault(c => c.CustomerId == id).UserName;
      }

      if (orderProducts.ToList().Count == 0)
      {
        OrderView orderViewNone = new OrderView();
        orderViewNone.EmptyMessage = "There are no orders for that Customer";
        orderViewNone.IsEmpty = true;
        orderViewNone.Customers = customers;
        return View("CustomerOrders", orderViewNone);
      }

      var inboth = (from p1 in _db.Orders
                    join p2 in orderProducts
                    on p1.OrderId equals p2.OrderId
                    select p1).Distinct();
      var res = inboth.ToList();

      OrderView orderView = new OrderView();
      orderView.Orders = res;
      orderView.Customers = customers;


      //return RedirectToAction("StoreOrders", new { id });
      return View("CustomerOrders", orderView);

    }

    public IActionResult CustomerOrders()
    {
      ViewData["CustomerOrders"] = "active";

      OrderView orderView = new OrderView();
      List<Order> orders = new List<Order>();
      var customers = _db.Customers.ToList();
      orderView.Orders = orders;
      orderView.Customers = customers;

      return View(orderView);
    }
  }
}
