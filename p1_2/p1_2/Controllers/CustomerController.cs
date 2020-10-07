using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using p1_2.Data;
using p1_2.Models;

namespace p1_2.Controllers
{
  public class CustomerController : Controller
  {
    private readonly BookopolisDbContext _db;
    private readonly IMemoryCache _cache;

    public CustomerController(BookopolisDbContext db, IMemoryCache cache)
    {
      _db = db;
      _cache = cache;
    }

    public IActionResult Login()
    {
      _cache.Set("shoppingCart", new List<ShoppingCart>());
      _cache.Set("LoggedInCustomer", new Customer());
      return View();
    }

    [HttpPost]
    public IActionResult LoginCustomer([Bind("UserName,Password")] Customer customer)
    {
      if (_db.Customers.Any(c => c.UserName == customer.UserName && c.Password == customer.Password))
      {
        ViewData["IsLoggedIn"] = "true";
        Customer c = _db.Customers.FirstOrDefault(c => c.UserName == customer.UserName && c.Password == customer.Password);
        _cache.Set("LoggedInCustomer", c);
        return RedirectToAction("LoadCustomer", "Store", customer);
      }

      ModelState.AddModelError("Password", "Incorrect username or password");
      return View("Login");
    }

    public IActionResult Signup()
    {
      _cache.Set("shoppingCart", new List<ShoppingCart>());
      return View();
    }

    [HttpPost]
    public IActionResult SignupCustomer([Bind("FirstName,LastName,UserName,Password")] Customer customer)
    {
      if (!ModelState.IsValid)
      {
        return View("Signup");
      }

      if (_db.Customers.Any(c => c.UserName == customer.UserName))
      {
        ModelState.AddModelError("UserName", "That username is taken");
        return View("Signup");
      }

      _db.Customers.Add(customer);
      _db.SaveChanges();

      return View("Login");
    }

    public IActionResult SearchCustomers(CustomerView customerV)
    {

      if (customerV.FirstName == null)
      {
        List<Customer> customers = new List<Customer>();
        CustomerView customerView = new CustomerView();
        customerView.Customers = customers;

        return View(customerView);
      }
      else
      {
        var customers = _db.Customers.Where(c => c.FirstName == customerV.FirstName).ToList();
        CustomerView customerView = new CustomerView();
        customerView.Customers = customers;

        return View(customerView);
      }

    }

    [HttpPost]
    public IActionResult DisplayCustomers([Bind("FirstName")] Customer customer)
    {

      CustomerView customerView = new CustomerView();
      customerView.FirstName = customer.FirstName;

      return RedirectToAction("SearchCustomers", customerView);

    }

    public JsonResult GetNames()
    {
      List<string> names = _db.Customers.Select(c => c.FirstName).ToList();

      return Json(names);
    }
  }
}
