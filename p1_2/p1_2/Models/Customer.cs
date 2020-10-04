using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace p1_2.Models
{
  public class Customer
  {
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Please enter your first name")]
    [StringLength(20, ErrorMessage = "The first name cannot be longer than 20 letters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please enter your last name")]
    [StringLength(20, ErrorMessage = "The last name cannot be longer than 20 letters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter your username")]
    [StringLength(20, ErrorMessage = "The username cannot be longer than 20 letters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    //[RegularExpression(".*[0-9A-Z].*", ErrorMessage = "Your password must have a number and a capital letter")]
    //[StringLength(25, ErrorMessage = "The minimum length of your password is 6 and the maximum is 25", MinimumLength = 6)]
    public string Password { get; set; }

    public List<CustomerAddress> CustomerAddresses { get; set; }
  }
}
