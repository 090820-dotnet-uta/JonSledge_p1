using System.Collections.Generic;
using p1_2.Models;

namespace p1_2.Interfaces
{
  interface IViewModel
  {
    public CheckoutView CreateCheckoutView(List<ShoppingCart> shoppingCarts);

    public List<OrderProduct> CreateOrderProducts(List<ShoppingCart> shoppingCart, Customer customer, CustomerAddress customerAddress);

    public ProductView CreateProductView(List<ShoppingCart> shoppingCartInvs, Inventory inv, Product prod);

    public IEnumerable<ProductView> CreateProductViews(List<Product> prodList, List<Inventory> inventories);

    public ShoppingCart CreateShoppingCart(ProductView productView, int storeId, string state);
  }
}
