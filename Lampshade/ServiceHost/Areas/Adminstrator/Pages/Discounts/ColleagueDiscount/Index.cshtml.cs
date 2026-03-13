using DiscountManagement.Application.Contracts.ColleagueDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.ProductAgg;

namespace ServiceHost.Areas.Adminstrator.Pages.Discounts.ColleagueDiscount;

public class IndexModel : PageModel
{
    public static bool WatchDeleted;
    private readonly IColleagueDiscountApplication _colleagueDiscountApplication;
    private readonly IProductApplication _productApplication;
    public List<ColleagueDiscountViewModel> ColleagueDiscounts;
    public SelectList Products;
    public ColleagueDiscountSearchModel SearchModel;
    public bool watch;

    public IndexModel(IColleagueDiscountApplication colleagueDiscountApplication,
        IProductApplication productApplication)
    {
        _productApplication = productApplication;
        _colleagueDiscountApplication = colleagueDiscountApplication;
    }

    [TempData] public string Message { get; set; }

    //[NeedsPermission(ShopPermissions.ListProducts)]
    public void OnGet(ColleagueDiscountSearchModel searchModel)
    {
        // Ensure SearchModel is not null so the view's asp-for bindings work even when user leaves fields empty
        SearchModel = searchModel ?? new ColleagueDiscountSearchModel();
        var allProducts = _productApplication.GetProducts();
        Products = new SelectList(allProducts, "Id", "Name");
        ColleagueDiscounts = _colleagueDiscountApplication.Search(SearchModel, WatchDeleted);
        watch = WatchDeleted;
    }

    public IActionResult OnGetCreate()
    {
        var command = new DefineColleagueDiscount
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
    public JsonResult OnPostCreate(DefineColleagueDiscount command)
    {
        var result = _colleagueDiscountApplication.Define(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var product = _colleagueDiscountApplication.GetDetails(id);
        product.Products = _productApplication.GetProducts();
        return Partial("Edit", product);
    }

    //[NeedsPermission(ShopPermissions.EditProduct)]
    public JsonResult OnPostEdit(EditColleagueDiscount command)
    {
        var result = _colleagueDiscountApplication.Edit(command);
        return new JsonResult(result);
    }

    public RedirectToPageResult OnGetRemove(long id)
    {
        _colleagueDiscountApplication.Remove(id);
        return RedirectToPage();
    }

    public RedirectToPageResult OnGetRestore(long id)
    {
        _colleagueDiscountApplication.Restore(id);
        return RedirectToPage("./Index", OnGetDeleted());
    }
}