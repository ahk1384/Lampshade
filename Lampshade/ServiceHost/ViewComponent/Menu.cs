using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponent;

public class Menu : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly IProductCategoryQuery _productCategory;
    public MenuModel Model { get; set; } = new MenuModel();
    public Menu(IProductCategoryQuery productCategory)
    {
        _productCategory = productCategory;
    }

    public IViewComponentResult Invoke()
    {
        Model.ProductCategories = _productCategory.GetProductCategories();
        return View(Model);
    }
}