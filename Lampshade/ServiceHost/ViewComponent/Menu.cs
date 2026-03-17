using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;
using ICookieManager = _01_LampshadeQuery.ICookieManager;

namespace ServiceHost.ViewComponent;

public class Menu : Microsoft.AspNetCore.Mvc.ViewComponent
{
    public const string CookieName = "cart-items";
    private readonly IArticleCategoryQuery _articleCategory;
    private readonly ICookieManager _cookieManager;
    private readonly IProductCategoryQuery _productCategory;

    public Menu(IProductCategoryQuery productCategory, IArticleCategoryQuery articleCategory,
        ICookieManager cookieManager)
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