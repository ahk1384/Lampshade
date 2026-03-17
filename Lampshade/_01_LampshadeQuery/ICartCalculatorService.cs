using ShopManagement.Application.Contracts.Cart;

namespace _01_LampshadeQuery;

public interface ICartCalculatorService
{
    CartViewModel ComputeCart(List<CartItemViewModel> cartItems);

    // Cart ComputeCart(List<CartItem> cartItems);
    CartItemViewModel ComputeCartItem(CartItemViewModel cartItemViewModel);
}