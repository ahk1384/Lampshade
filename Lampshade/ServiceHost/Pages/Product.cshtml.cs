using _0_Framework.Application;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.Product;
using CommentManagement.Application.Contracts.Comment;
using CommnetManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Cart;

namespace ServiceHost.Pages;

public class ProductModel : PageModel
{
    public const string CookieName = "cart-items";
    private readonly IAuthHelper _authHelper;
    private readonly ICartCalculatorService _cartCalculatorService;
    private readonly ICartQuery _cartQuery;
    private readonly ICommentApplication _commentApplication;
    private readonly ICookieManager _cookieManager;
    private readonly IProductQuery _productQuery;
    public ProductQueryModel Product;

    public ProductModel(IProductQuery product, IProductQuery productQuery, ICommentApplication commentApplication,
        ICartQuery cartQuery, IAuthHelper authHelper, ICookieManager cookieManager,
        ICartCalculatorService cartCalculatorService)
    {
        _productQuery = productQuery;
        _commentApplication = commentApplication;
        _cartQuery = cartQuery;
        _authHelper = authHelper;
        _cookieManager = cookieManager;
        _cartCalculatorService = cartCalculatorService;
        _productQuery = product;
    }


    public IActionResult OnGet(string id)
    {
        Product = _productQuery.GetProductDetails(id);
        if (Product == null)
        {
            return RedirectToPage("/Index");
        }

        return null;
    }

    public IActionResult OnPost(AddComment command, string productSlug)
    {
        command.Type = CommentType.Product;
        if (_authHelper.IsAuthenticated()) command.Name = _authHelper.CurrentAccountInfo().Username;
        command.Rating = command.Rating == 0 ? 1 : command.Rating;
        var result = _commentApplication.Add(command);
        return RedirectToPage("/Product", new { Id = productSlug });
    }

    public IActionResult OnPostAddToCart(string id, int count)
    {
        var Product = _productQuery.GetProductDetails(id);

        if (_authHelper.IsAuthenticated())
        {
            var item = new CartItemViewModel(Product.Id, Product.Name, Product.DoublePrice, Product.Picture,
                count, Product.IsInStock,
                Product.DiscountRate, Product.Slug);
            item = _cartCalculatorService.ComputeCartItem(item);
            _cartQuery.AddToCart(item, _authHelper.CurrentAccountInfo().Id);
        }

        _cookieManager.Merge(Response);

        return RedirectToPage();
    }
}