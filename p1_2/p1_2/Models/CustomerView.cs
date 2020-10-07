using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace p1_2.Models
{
  public class CustomerView
  {
    public int CustomerViewId { get; set; }

    public Customer Customer { get; set; }

    [Required(ErrorMessage = "Please enter a first name")]
    [StringLength(20, ErrorMessage = "The first name cannot be longer than 20 letters")]
    public string FirstName { get; set; }

    public List<Customer> Customers { get; set; }
  }
}
