namespace ShopManagement.Application.Contracts.Cart;

public interface ICartService
{
    CartViewModel Get();
    void Set(CartViewModel cart);
}