using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponent;

public class ProductCategoryViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly IProductCategoryQuery _productCategoryQuery;

    public ProductCategoryViewComponent(IProductCategoryQuery productCategoryQuery)
    {
        _productCategoryQuery = productCategoryQuery;
    }

    public IViewComponentResult Invoke()
    {
        var productCategories = _productCategoryQuery.GetProductCategories();
        return View(productCategories);
    }
}