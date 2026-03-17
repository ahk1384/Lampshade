using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategoryAgg;

namespace ServiceHost.Pages;

public class IndexModel : PageModel
{
    public List<ProductCategoryViewModel> ProductCategories { get; set; }


    public void OnGet(int run)
    {
    }
}