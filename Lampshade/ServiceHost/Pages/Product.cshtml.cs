using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.Product;
using CommentManagement.Application.Contracts.Comment;
using CommnetManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Cart;
using ShopManagementDomain.CartAgg;
using ShopManagementDomain.ProductAgg;

namespace ServiceHost.Pages;

public class ProductModel : PageModel
{
    private readonly ICommentApplication _commentApplication;
    private readonly IProductQuery _productQuery;
    private readonly ICartQuery _cartQuery;
    private readonly    IAuthHelper _authHelper;
    public ProductQueryModel Product;
    public const string CookieName = "cart-items";

    public ProductModel(IProductQuery product, IProductQuery productQuery, ICommentApplication commentApplication, ICartQuery cartQuery, IAuthHelper authHelper)
    {
        _productQuery = productQuery;
        _commentApplication = commentApplication;
        _cartQuery = cartQuery;
        _authHelper = authHelper;
        _productQuery = product;
    }


    public void OnGet(string id)
    {
        Product = _productQuery.GetProductDetails(id);
    }

    public IActionResult OnPost(AddComment command, string productSlug)
    {
        command.Type = CommentType.Product;
        if (_authHelper.IsAuthenticated())
        {
            command.Name = _authHelper.CurrentAccountInfo().Username;
        }
        var result = _commentApplication.Add(command);
        return RedirectToPage("/Product", new { Id = productSlug });
    }

    public IActionResult OnPostAddToCart(string id,int count)
    {
        var Product = _productQuery.GetProductDetails(id);
        if (_authHelper.IsAuthenticated())
        {
            _cartQuery.AddToCart(new CartItemViewModel(Product.Id,Product.Name, Product.DoublePrice, Product.Picture, count, Product.IsInStock,
                Product.DiscountRate),_authHelper.CurrentAccountInfo().Id);
            var cartitems = _cartQuery.GetCart(_authHelper.CurrentAccountInfo().Id).Items;
            var searlizer = new JavaScriptSerializer();
            var CartItems = searlizer.Serialize(cartitems);
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2) };
            Response.Cookies.Delete(CookieName);
            Response.Cookies.Append(CookieName, CartItems, options);
        }
        return RedirectToPage();
    }
}