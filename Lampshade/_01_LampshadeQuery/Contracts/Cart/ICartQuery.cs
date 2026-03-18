using _0_Framework.Application;
using ShopManagement.Application.Contracts.Cart;

namespace _01_LampshadeQuery.Contracts.Cart;

public interface ICartQuery
{
    CartViewModel GetCart(long accountId);
    OperationResult AddToCart(CartItemViewModel item, long accountId);
    OperationResult AddAllToCart(List<CartItemViewModel> item, long accountId);
    OperationResult ChangeItemCount(CartItemViewModel item, long accountId);
    OperationResult RemoveFromCart(long productId, long accountId);
    OperationResult RemoveCart(long accountId);
}