using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using p1_2.Data;
using p1_2.DbManip;
using p1_2.Models;
using p1_2.Utils;

namespace p1_2.Controllers
{
  public class OrderController : Controller
  {
    private readonly IMemoryCache _cache;
    private readonly BookopolisDbContext _db;
    public OrderController(IMemoryCache cache, BookopolisDbContext db)
    {
      _cache = cache;
      _db = db;
    }

    public IActionResult StoreOrders(int id)
    {
      if (!Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }
      ViewData["StoreOrders"] = "active";
      ViewModel viewModel = new ViewModel();
      var orderProducts = DbManipulation.GetOrdersFromStore(_db, id);
      List<Store> stores = DbManipulation.GetStores(_db).ToList();

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
        OrderView orderViewNone = viewModel.CreateEmptyOrderView(stores);
        return View(orderViewNone);
      }

      var orders = DbManipulation.GetOrdersFromOrderProducts(_db, orderProducts);

      OrderView orderView = viewModel.CreateOrderView(orders, stores);
      return View(orderView);
    }

    public IActionResult MyOrders()
    {
      if (!Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }
      ViewData["MyOrders"] = "active";
      ViewModel viewModel = new ViewModel();
      Customer tempCust = (Customer)_cache.Get("LoggedInCustomer");

      IEnumerable<OrderProduct> orderProducts = DbManipulation.GetCustomerOrderProducts(_db, tempCust.CustomerId);

      List<Order> orders = DbManipulation.GetOrdersFromOrderProducts(_db, orderProducts);

      OrderView orderView = viewModel.CreateOrderView(orders);
      return View(orderView);
    }

    public IActionResult CustomerOrders(int? id)
    {
      if (!Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }
      ViewData["CustomerOrders"] = "active";
      ViewModel viewModel = new ViewModel();
      var orderProducts = DbManipulation.GetCustomerOrderProducts(_db, id);
      var customers = DbManipulation.GetCustomers(_db).ToList();

      if (id == 0 || id == null)
      {
        ViewData["CurrentCustomer"] = "none";
      }
      else
      {
        ViewData["CurrentCustomer"] = customers.FirstOrDefault(c => c.CustomerId == id).UserName;
      }

      if (orderProducts.ToList().Count == 0)
      {
        OrderView orderViewNone = viewModel.CreateEmptyOrderView(customers);
        return View("CustomerOrders", orderViewNone);
      }

      List<Order> orders = DbManipulation.GetOrdersFromOrderProducts(_db, orderProducts);
      OrderView orderView = viewModel.CreateOrderView(orders, customers);

      return View("CustomerOrders", orderView);
    }
  }
}
