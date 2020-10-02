using System.Collections.Generic;

namespace p1_2.Models
{
  public class CheckoutView
  {
    public int CheckoutViewId { get; set; }
    public CustomerAddress CustomerAddress { get; set; }
    public List<ShoppingCart> ShoppingCarts { get; set; }
  }
}
