﻿
namespace p1_2.Models
{
  public class ProductView
  {
    public int ProductViewId { get; set; }

    public int ProductId { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public int Amount { get; set; }

    public bool IsInCart { get; set; }

    public string State { get; set; }

    public string ImgUrl { get; set; }
  }
}
