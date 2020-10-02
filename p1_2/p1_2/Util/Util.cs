using Microsoft.Extensions.Caching.Memory;

namespace p1_2.Util
{
  public static class Util
  {
    public static bool IsLoggedIn(IMemoryCache cache)
    {
      return false;
      //return (cache.Get("UserName") == null);
    }

  }
}
