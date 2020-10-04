using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using p1_2.Data;
using System.Linq;

namespace p1_2.Utils
{
  public static class Util
  {
    public static bool IsLoggedIn(IMemoryCache cache)
    {
      return false;
      //return (cache.Get("UserName") == null);
    }

    public static bool IsSeeded(BookopolisDbContext _db)
    {
      return (_db.Products.Count() > 0 && _db.Stores.Count() > 0);
    }

  }
}
