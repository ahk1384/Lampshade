using ShopManagement.Application.Contracts.Cart;

namespace ShopManagement.Application;

public class CartService : ICartService
{
    public CartViewModel Cart { get; set; }

    public CartViewModel Get()
    {
        return Cart;
    }

    public void Set(CartViewModel cart)
    {
        Cart = cart;
    }
}