using System.Collections.Generic;

namespace p1_2.Models
{
  public class CustomerAddress
  {
    public int CustomerAddressId { get; set; }

    public string StreetAddress { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string ZIP { get; set; }

    public List<OrderProduct> OrderProducts { get; set; }

    public int CustomerId { get; set; }
  }
}
