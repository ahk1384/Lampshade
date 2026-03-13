using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponent;

public class Menu : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly IArticleCategoryQuery _articleCategory;
    private readonly IProductCategoryQuery _productCategory;

    public Menu(IProductCategoryQuery productCategory, IArticleCategoryQuery articleCategory)
    {
        _productCategory = productCategory;
        _articleCategory = articleCategory;
    }

    public MenuModel Model { get; set; } = new();

    public IViewComponentResult Invoke()
    {
        Model.ProductCategories = _productCategory.GetProductCategories();
        Model.ArticleCategories = _articleCategory.GetArticleCategories();
        return View(Model);
    }
}