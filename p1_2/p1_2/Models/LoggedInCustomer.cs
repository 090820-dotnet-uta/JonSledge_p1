﻿
namespace p1_2.Models
{
  public class LoggedInCustomer
  {
    public int LoggedInCustomerId { get; set; }
    public string UserName { get; set; }
    public bool IsLoggedIn { get; set; } = false;
  }
}
