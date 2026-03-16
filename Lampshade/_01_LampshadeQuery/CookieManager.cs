using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_LampshadeQuery.Contracts.Cart;
using Microsoft.AspNetCore.Http;
using Nancy.Json;
using ShopManagement.Application.Contracts.Cart;

namespace _01_LampshadeQuery;

public class CookieManager : ICookieManager
{
    private readonly ICartQuery _cartQuery;
    private readonly IAuthHelper _authHelper;
    public const string CookieName = "cart-items";

    public CookieManager(ICartQuery cartQuery, IAuthHelper authHelper)
    {
        _cartQuery = cartQuery;
        _authHelper = authHelper;
    }

    public void Merge(HttpResponse response)
    {
        if (_authHelper.IsAuthenticated())
        {
            var serializer = new JavaScriptSerializer();
            var id = _authHelper.CurrentAccountInfo().Id;
            var w = _cartQuery.GetCart(id);
            if (w != null && w.Items.Count > 0)
            {
                var cartitems = w.Items;
                var searlizer = new JavaScriptSerializer();
                var CartItems = searlizer.Serialize(cartitems);
                var options = new CookieOptions { Expires = DateTime.Now.AddDays(2) };
                response.Cookies.Append(CookieName, CartItems, options);
            }
            else
            {
                response.Cookies.Delete(CookieName);
            }
        }
    }

    public List<CartItemViewModel> GetCartItems(HttpRequest request)
    {
        var serializer = new JavaScriptSerializer();
        var value = request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItemViewModel>>(value);
        if(cartItems != null && cartItems.Count > 0)
            return cartItems;
        return new List<CartItemViewModel>();
    }
    
}