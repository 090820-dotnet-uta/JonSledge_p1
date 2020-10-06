using System.Collections.Generic;
using p1_2.Interfaces;
using p1_2.Models;

namespace p1_2.Utils
{
  public class ViewModel : IViewModel
  {
    public CheckoutView CreateCheckoutView(List<ShoppingCart> shoppingCarts)
    {
      CheckoutView checkoutView = new CheckoutView();
      checkoutView.ShoppingCarts = shoppingCarts;
      checkoutView.CustomerAddress = new CustomerAddress();
      return checkoutView;
    }

    public IEnumerable<ProductView> CreateProductViews(List<Product> prodList, List<Inventory> inventories)
    {
      List<ProductView> prodViews = new List<ProductView>();
      for (int i = 0; i < inventories.Count; i++)
      {
        ProductView productView = new ProductView()
        {
          Amount = inventories[i].Amount,
          Author = prodList[i].Author,
          Description = prodList[i].Description,
          Price = prodList[i].Price,
          ProductId = prodList[i].ProductId,
          Title = prodList[i].Title
        };
        prodViews.Add(productView);
      }
      IEnumerable<ProductView> EProdViews = prodViews;
      return EProdViews;
    }

    public ProductView CreateProductView(List<ShoppingCart> shoppingCartInvs, Inventory inv, Product prod)
    {
      ProductView productView = new ProductView();

      productView.Amount = shoppingCartInvs.Count > 0 ? shoppingCartInvs[shoppingCartInvs.Count - 1].StockAmount : inv.Amount;
      productView.Author = prod.Author;
      productView.Description = prod.Description;
      productView.Price = prod.Price;
      productView.ProductId = prod.ProductId;
      productView.Title = prod.Title;
      productView.IsInCart = (shoppingCartInvs.Count == 2);

      return productView;
    }

    public ShoppingCart CreateShoppingCart(ProductView productView, int storeId, string state)
    {
      productView.Amount--;
      ShoppingCart sh = new ShoppingCart()
      {
        StockAmount = productView.Amount,
        Author = productView.Author,
        Title = productView.Title,
        Price = productView.Price,
        StoreId = storeId,
        ProductId = productView.ProductId,
        State = state
      };

      return sh;
    }
    public OrderView CreateOrderView(List<Order> orders)
    {
      OrderView orderView = new OrderView();
      orderView.Orders = orders;
      return orderView;
    }

    public OrderView CreateOrderView(List<Order> orders, List<Store> stores)
    {
      OrderView orderView = new OrderView();
      orderView.Orders = orders;
      orderView.Stores = stores;
      return orderView;
    }

    public OrderView CreateOrderView(List<Order> orders, List<Customer> customers)
    {
      OrderView orderView = new OrderView();
      orderView.Orders = orders;
      orderView.Customers = customers;
      return orderView;
    }

    public OrderView CreateEmptyOrderView(List<Store> stores)
    {
      OrderView orderViewNone = new OrderView();
      orderViewNone.EmptyMessage = "There are no orders for that state";
      orderViewNone.IsEmpty = true;
      orderViewNone.Stores = stores;
      return orderViewNone;
    }

    public OrderView CreateEmptyOrderView(List<Customer> customers)
    {
      OrderView orderViewNone = new OrderView();
      orderViewNone.EmptyMessage = "There are no orders for that Customer";
      orderViewNone.IsEmpty = true;
      orderViewNone.Customers = customers;
      return orderViewNone;
    }

  }
}
