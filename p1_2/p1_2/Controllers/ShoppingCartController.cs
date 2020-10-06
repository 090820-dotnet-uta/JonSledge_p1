using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using p1_2.Data;
using p1_2.Utils;
using p1_2.Models;
using p1_2.DbManip;

namespace p1_2.Controllers
{
  public class ShoppingCartController : Controller
  {
    private readonly IMemoryCache _cache;//must set this for DI in Startup.cs
    private readonly BookopolisDbContext _db;
    private List<ShoppingCart> shoppingCart;
    public ShoppingCartController(IMemoryCache cache, BookopolisDbContext db)
    {
      _cache = cache;
      _db = db;

      if (!_cache.TryGetValue("shoppingCart", out shoppingCart))
      {
        _cache.Set("shoppingCart", new List<ShoppingCart>());
        _cache.TryGetValue("shoppingCart", out shoppingCart);
      }
    }

    public IActionResult Index()
    {
      if (!Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }
      return View(shoppingCart);
    }

    public IActionResult Delete(ShoppingCart sh)
    {
      if (Util.GetShoppingCartStoreProducts(shoppingCart, sh).Count == 1)
      {
        ShoppingCart shop = Util.GetShoppingCartStoreProducts(shoppingCart, sh).FirstOrDefault();
        shoppingCart.Remove(shop);
        _cache.Set("shoppingCart", shoppingCart);
        return RedirectToAction("Index");
      }
      else
      {
        List<ShoppingCart> shops = Util.GetShoppingCartStoreProducts(shoppingCart, sh);
        ShoppingCart shopToRemove = Util.FindAndRemoveShoppingCart(shops);
        shoppingCart.Remove(shopToRemove);
        _cache.Set("shoppingCart", shoppingCart);
        return RedirectToAction("Index");
      }
    }

    public IActionResult Checkout()
    {
      if (!Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }

      ViewModel viewModel = new ViewModel();
      ViewData["CheckoutTotal"] = shoppingCart.Sum(s => s.Price);
      CheckoutView checkoutView = viewModel.CreateCheckoutView(shoppingCart);

      return View(checkoutView);
    }

    [HttpPost]
    public IActionResult PlaceOrder([Bind("StreetAddress,Country,City,State,ZIP")] CustomerAddress customerAddress)
    {
      Customer tempCust = (Customer)_cache.Get("LoggedInCustomer");
      Customer customer = DbManipulation.GetCustomer(_db, tempCust.CustomerId);
      List<CustomerAddress> customerAddresses = DbManipulation.GetCustomerAddresses(_db, tempCust.CustomerId).ToList();


      if (customerAddresses.Count > 0)
      {
        customerAddress = Util.CheckForRepeatAddress(customerAddresses, customerAddress);
      }

      Dictionary<int, int> myDict = Util.GetShoppingCartInventoryAmounts(shoppingCart);
      DbManipulation.UpdateInventoryAmounts(_db, myDict);

      ViewModel viewModel = new ViewModel();

      Order order = new Order();
      order.TimeOfOrder = DateTime.Now;
      order.OrderProducts = Util.CreateOrderProducts(shoppingCart, customer, customerAddress);
      order.Total = shoppingCart.Sum(sh => sh.Price);
      List<Order> orders = new List<Order>();
      orders.Add(order);

      customerAddresses.Add(customerAddress);

      //add new address to customer
      customer.CustomerAddresses = customerAddresses;


      _db.Orders.Add(order);
      _db.SaveChanges();

      _cache.Set("shoppingCart", new List<ShoppingCart>());
      shoppingCart = new List<ShoppingCart>();
      return RedirectToAction("MyOrders", "Order");

    }

  }
}
