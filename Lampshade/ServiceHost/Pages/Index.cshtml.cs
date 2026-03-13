using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategoryAgg;

namespace ServiceHost.Areas.Adminstrator.Pages.Shop.ProductCategories;

public class IndexModel : PageModel
{
    private readonly IProductCategoryApplication _productCategoryApplication;

    public IndexModel(IProductCategoryApplication productCategoryApplication)
    {
        _productCategoryApplication = productCategoryApplication;
    }

    public List<ProductCategoryViewModel> ProductCategories { get; set; }


    public void OnGet()
    {
        //ProductCategories = _productCategoryApplication.GetAll();
    }
}