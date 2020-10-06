using System;
using System.Collections.Generic;

namespace p1_2.Models
{
  public class Order
  {
    public int OrderId { get; set; }

    public DateTime TimeOfOrder { get; set; }

    public double Total { get; set; }

    public List<OrderProduct> OrderProducts { get; set; }
  }
}
