using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using p1_2.Data;
using p1_2.Models;
using p1_2.DbManip;

namespace p1_2.Controllers
{
  public class HomeController : Controller
  {
    private readonly BookopolisDbContext _db;

    public HomeController(BookopolisDbContext db)
    {
      _db = db;
    }

    public IActionResult Index()
    {
      if (!DbManipulation.IsSeeded(_db))
      {
        DbManipulation.SeedDb(_db);
      }

      ViewData["IsLoggedIn"] = "false";
      return View("Index");
    }

    public IActionResult Privacy()
    {
      return View("Privacy");
    }

    [HttpPost]
    public IActionResult HandlePost([FromBody] Customer customer)
    {
      var x = customer;
      return RedirectToAction("Privacy");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
