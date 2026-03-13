using _0_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.ProductAgg;
using ShopManagement.Application.Contracts.ProductCategoryAgg;

namespace ServiceHost.Areas.Adminstrator.Pages.Shop.Product;

public class IndexModel : PageModel
{
    public static bool WatchDeleted;
    private readonly IProductApplication _productApplication;
    private readonly IProductCategoryApplication _productCategoryApplication;
    public SelectList ProductCategories;
    public List<ProductViewModel> Products;
    public ProductSearchModel SearchModel;
    public bool watch;


    public IndexModel(IProductApplication productApplication,
        IProductCategoryApplication productCategoryApplication)
    {
        _productApplication = productApplication;
        _productCategoryApplication = productCategoryApplication;
    }

    [TempData] public string Message { get; set; }

    //[NeedsPermission(ShopPermissions.ListProducts)]

    public void OnGet(ProductSearchModel searchModel)
    {
        ProductCategories = new SelectList(_productCategoryApplication.GetProductCategories(), "Id", "Title");
        Products = _productApplication.Search(searchModel, WatchDeleted);
        watch = WatchDeleted;
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateProduct
        {
            Categories = _productCategoryApplication.GetProductCategories()
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
    public JsonResult OnPostCreate(CreateProduct command)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();

            var message = errors.Any()
                ? string.Join(" | ", errors)
                : "Validation failed";

            return new JsonResult(new OperationResult().Fail(message));
        }

        var result = _productApplication.Add(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var product = _productApplication.GetDetails(id);
        product.Categories = _productCategoryApplication.GetProductCategories();
        return Partial("Edit", product);
    }

    //[NeedsPermission(ShopPermissions.EditProduct)]
    public JsonResult OnPostEdit(EditProduct command)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();

            var message = errors.Any()
                ? string.Join(" | ", errors)
                : "Validation failed";

            return new JsonResult(new OperationResult().Fail(message));
        }

        var result = _productApplication.Edit(command);
        return new JsonResult(result);
    }

    public RedirectToPageResult OnGetRemove(long id)
    {
        _productApplication.Remove(id);
        return RedirectToPage();
    }

    public RedirectToPageResult OnGetRestore(long id)
    {
        _productApplication.Restore(id);
        return RedirectToPage("./Index", OnGetDeleted());
    }
}