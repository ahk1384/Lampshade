using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ICookieManager = _01_LampshadeQuery.ICookieManager;

namespace ServiceHost.ViewComponent;

public class Menu : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly IArticleCategoryQuery _articleCategory;
    private readonly IProductCategoryQuery _productCategory;
    private readonly ICookieManager _cookieManager;
    public const string CookieName = "cart-items";

    public Menu(IProductCategoryQuery productCategory, IArticleCategoryQuery articleCategory, ICookieManager cookieManager)
    {
        _productCategory = productCategory;
        _articleCategory = articleCategory;
        _cookieManager = cookieManager;
    }

    public MenuModel Model { get; set; } = new();

    public IViewComponentResult Invoke()
    {
        Model.ProductCategories = _productCategory.GetProductCategories();
        Model.ArticleCategories = _articleCategory.GetArticleCategories();
        _cookieManager.Merge(HttpContext.Response);
        return View(Model);
    }
}