using DiscountManagement.Application.Contracts.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.ProductAgg;

namespace ServiceHost.Areas.Adminstrator.Pages.Discounts.CustomerDiscount;

public class IndexModel : PageModel
{
    public static bool WatchDeleted;
    private readonly ICustomerDiscountApplication _customerDiscountApplication;
    private readonly IProductApplication _productApplication;
    public List<CustomerDiscountViewModel> CustomerDiscounts;
    public SelectList Products;
    public CustomerDiscountSearchModel SearchModel;
    public bool watch;

    public IndexModel(ICustomerDiscountApplication customerDiscountApplication, IProductApplication productApplication)
    {
        _productApplication = productApplication;
        _customerDiscountApplication = customerDiscountApplication;
    }

    [TempData] public string Message { get; set; }

    //[NeedsPermission(ShopPermissions.ListProducts)]
    public void OnGet(CustomerDiscountSearchModel searchModel)
    {
        // Ensure SearchModel is not null so the view's asp-for bindings work even when user leaves fields empty
        SearchModel = searchModel ?? new CustomerDiscountSearchModel();

        var allProducts = _productApplication.GetProducts();
        Products = new SelectList(allProducts, "Id", "Name");
        CustomerDiscounts = _customerDiscountApplication.Search(SearchModel, WatchDeleted);
        watch = WatchDeleted;
    }

    public IActionResult OnGetCreate()
    {
        var command = new DefineCustomerDiscount
        {
            Products = _productApplication.GetProducts()
        };
        return Partial("./Create", command);
    }

    public RedirectToPageResult OnGetDeleted()
    {
        WatchDeleted = true;
        return RedirectToPage();
    }

    public RedirectToPageResult OnGetActive()
    {
        WatchDeleted = false;
        return RedirectToPage();
    }

    //[NeedsPermission(ShopPermissions.CreateProduct)]
    public JsonResult OnPostCreate(DefineCustomerDiscount command)
    {
        var result = _customerDiscountApplication.Define(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var product = _customerDiscountApplication.GetDetails(id);
        product.Products = _productApplication.GetProducts();
        return Partial("Edit", product);
    }

    //[NeedsPermission(ShopPermissions.EditProduct)]
    public JsonResult OnPostEdit(EditCustomerDiscount command)
    {
        var result = _customerDiscountApplication.Edit(command);
        return new JsonResult(result);
    }

    public RedirectToPageResult OnGetRemove(long id)
    {
        _customerDiscountApplication.Remove(id);
        return RedirectToPage();
    }

    public RedirectToPageResult OnGetRestore(long id)
    {
        _customerDiscountApplication.Restore(id);
        return RedirectToPage("./Index", OnGetDeleted());
    }
}