using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.ProductCategoryAgg;

namespace ServiceHost.Pages;

public class IndexModel : PageModel
{

    public IndexModel()
    {
    }

    public List<ProductCategoryViewModel> ProductCategories { get; set; }


    public void OnGet(int run)
    {
    }
}