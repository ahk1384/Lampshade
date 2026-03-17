using Microsoft.AspNetCore.Http;
using ShopManagement.Application.Contracts.Cart;

namespace _01_LampshadeQuery;

public interface ICookieManager
{
    void Merge(HttpResponse response);
    List<CartItemViewModel> GetCartItems(HttpRequest request);
}