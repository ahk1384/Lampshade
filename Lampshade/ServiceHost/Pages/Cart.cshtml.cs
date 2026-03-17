using _0_Framework.Application;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Cart;

namespace ServiceHost.Pages;

[Authorize]
public class CartModel : PageModel
{
    public const string CookieName = "cart-items";
    private readonly IAuthHelper _authHelper;
    private readonly ICartCalculatorService _cartCalculatorService;
    private readonly ICartQuery _cartQuery;
    private readonly ICookieManager _cookieManager;
    private readonly IProductQuery _productQuery;
    public CartViewModel Cart;
    public List<CartItemViewModel> CartItems;


    public CartModel(IProductQuery productQuery, ICartQuery cartQuery, IAuthHelper authHelper,
        ICookieManager cookieManager, ICartCalculatorService cartCalculatorService)
    {
        _productQuery = productQuery;
        _cartQuery = cartQuery;
        _authHelper = authHelper;
        _cookieManager = cookieManager;
        _cartCalculatorService = cartCalculatorService;
    }

    public void OnGet()
    {
        CartItems = _cookieManager.GetCartItems(Request);
        if (CartItems.Count > 0)
        {
            CartItems = _productQuery.CheckInventoryStatus(CartItems);
            CartItems = _cartCalculatorService.ComputeCart(CartItems).Items;
            Cart = _cartCalculatorService.ComputeCart(CartItems);
        }
    }

    public IActionResult OnGetChangeItemCount(int productId, int count)
    {
        var items = _cookieManager.GetCartItems(Request);
        var id = _authHelper.CurrentAccountInfo().Id;
        var cartItem = items.FirstOrDefault(x => x.ProductId == productId);
        cartItem?.Count = count;
        _cartQuery.ChangeItemCount(cartItem, id);
        _cookieManager.Merge(Response);
        return Redirect("https://localhost:7199/Cart");
    }

    public IActionResult OnGetRemoveFromCart(long id)
    {
        _cartQuery.RemoveFromCart(id, _authHelper.CurrentAccountInfo().Id);
        _cookieManager.Merge(Response);
        return RedirectToPage();
    }

    public IActionResult OnGetGoToCheckOut()
    {
        var cartItems = _cookieManager.GetCartItems(Request);
        foreach (var item in cartItems) item.CalculateTotalItemPrice();

        CartItems = _productQuery.CheckInventoryStatus(cartItems);
        return RedirectToPage(CartItems.Any(x => !x.IsInStock) ? "/Cart" : "/Checkout");
    }
}