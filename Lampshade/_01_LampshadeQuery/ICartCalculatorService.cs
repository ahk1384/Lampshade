using ShopManagement.Application.Contracts.Cart;
using ShopManagementDomain.CartAgg;

namespace _01_LampshadeQuery;

public interface ICartCalculatorService
{
    CartViewModel ComputeCart(List<CartItemViewModel> cartItems);
}