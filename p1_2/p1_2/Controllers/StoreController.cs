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
  public class StoreController : Controller
  {
    private readonly BookopolisDbContext _db;
    private IMemoryCache _cache;//must set this for DI in Startup.cs

    public StoreController(BookopolisDbContext db, IMemoryCache cache)
    {
      _db = db;
      _cache = cache;
    }

    public IActionResult LoadLocalStorage()
    {
      //send username to local store GET request here!
      string s = _cache.Get("UserName").ToString();
      return new JsonResult(s);
    }

    public IActionResult Index()
    {
      if (Util.IsLoggedIn(_cache))
      {
        return RedirectToAction("Login", "Customer");
      }

      IEnumerable<Store> storeList = DbManipulation.GetStores(_db);
      return View(storeList);
    }

    public IActionResult LoadCustomer(Customer c)
    {
      _cache.Set("UserName", c.UserName);
      return RedirectToAction("Index");
    }

    public IActionResult StoreProducts(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      if (!DbManipulation.IsStore(_db, id))
      {
        return NotFound();
      }

      return RedirectToAction("Index", "Product", new { id });
    }

  }
}
