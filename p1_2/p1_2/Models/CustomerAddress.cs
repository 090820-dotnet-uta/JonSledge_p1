using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace p1_2.Models
{
  public class CustomerAddress
  {
    public int CustomerAddressId { get; set; }

    [Required(ErrorMessage = "Please enter your street address")]
    public string StreetAddress { get; set; }

    [Required(ErrorMessage = "Please enter your city")]
    public string City { get; set; }

    [Required(ErrorMessage = "Please enter your state")]
    public string State { get; set; }

    [Required(ErrorMessage = "Please enter your country")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Please enter your zipcode")]
    public string ZIP { get; set; }

    public List<OrderProduct> OrderProducts { get; set; }

    public int CustomerId { get; set; }
  }
}
