using _0_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.ProductAgg;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.ProductPictureAgg;

namespace ServiceHost.Areas.Adminstrator.Pages.Shop.ProductPictures;

public class IndexModel : PageModel
{
    private readonly IProductPictureApplication _productPictureApplication;
    private readonly IProductApplication _productApplication;
    public SelectList Products;
    public List<ProductPictureViewModel> ProductPictures;
    public ProductPictureSearchModel SearchModel;
    public static bool WatchDeleted = false;
    public bool watch = false;
    public IndexModel(IProductApplication productApplication, IProductPictureApplication productPictureApplication)
    {
        _productApplication = productApplication;
        _productPictureApplication = productPictureApplication;
    }

    [TempData] public string Message { get; set; }

    //[NeedsPermission(ShopPermissions.ListProducts)]
    public void OnGet(ProductPictureSearchModel searchModel)
    {
        // Ensure SearchModel is not null so the view's asp-for bindings work even when user leaves fields empty
        SearchModel = searchModel ?? new ProductPictureSearchModel();

        Products = new SelectList(_productApplication.GetProducts(), "Id", "Name");
        ProductPictures = _productPictureApplication.Search(SearchModel, WatchDeleted);
        watch = WatchDeleted;
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateProductPicture()
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
    public JsonResult OnPostCreate(CreateProductPicture command)
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
        var result = _productPictureApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var product = _productPictureApplication.GetDetails(id);
        product.Products = _productApplication.GetProducts();
        return Partial("Edit", product);
    }

    //[NeedsPermission(ShopPermissions.EditProduct)]
    public JsonResult OnPostEdit(EditProductPicture command)
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
        var result = _productPictureApplication.Edit(command);
        return new JsonResult(result);
    }
    public RedirectToPageResult OnGetRemove(long id)
    {
        _productPictureApplication.Remove(id);
        return RedirectToPage();
    }
    public RedirectToPageResult OnGetRestore(long id)
    {
        _productPictureApplication.Restore(id);
        return RedirectToPage("./Index", OnGetDeleted());
    }
}