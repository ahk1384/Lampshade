using _01_LampshadeQuery.Contracts.Cart;
using Microsoft.AspNetCore.Http;
using ShopManagement.Application.Contracts.Cart;
using ShopManagementDomain.CartAgg;

namespace _01_LampshadeQuery;

public interface ICookieManager
{
    void Merge(HttpResponse  response);
    List<CartItemViewModel> GetCartItems(HttpRequest request);
}