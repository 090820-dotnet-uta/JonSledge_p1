using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace p1_2.Models
{
  public class Customer
  {
    public int CustomerId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    public List<CustomerAddress> CustomerAddresses { get; set; }
  }
}
