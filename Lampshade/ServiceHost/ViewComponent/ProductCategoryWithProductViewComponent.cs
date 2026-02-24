using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponent;

public class ProductCategoryWithProductViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly IProductCategoryQuery _productCategoryQuery;

    public ProductCategoryWithProductViewComponent(IProductCategoryQuery productCategoryQuery)
    {
        _productCategoryQuery = productCategoryQuery;
    }

    public IViewComponentResult Invoke()
    {
        var categories = _productCategoryQuery.GetProductCategoriesWithProducts();
        return View(categories);
    }
}