﻿
namespace p1_2.Models
{
  public class OrderProduct
  {
    public int OrderProductId { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public int StoreId { get; set; }

    public int CustomerId { get; set; }

    public int CustomerAddressId { get; set; }

    public CustomerAddress CustomerAddress { get; set; }
  }
}
