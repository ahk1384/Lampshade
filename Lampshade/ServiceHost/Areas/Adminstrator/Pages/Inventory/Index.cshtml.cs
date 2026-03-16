using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.ProductAgg;

namespace ServiceHost.Areas.Adminstrator.Pages.Inventory;
[Authorize]
public class IndexModel : PageModel
{
    public static bool WatchDeleted;
    private readonly IInventoryApplication _inventoryApplication;
    private readonly IProductApplication _productApplication;
    private readonly IAuthHelper _authHelper;
    public List<InventoryViewModel> Inventories;
    public SelectList Products;
    public InventorySearchModel SearchModel;
    public bool watch;

    public IndexModel(IInventoryApplication inventoryApplication, IProductApplication productApplication, IAuthHelper authHelper)
    {
        _productApplication = productApplication;
        _authHelper = authHelper;
        _inventoryApplication = inventoryApplication;
    }

    [TempData] public string Message { get; set; }

    //[NeedsPermission(ShopPermissions.ListProducts)]
    public void OnGet(InventorySearchModel searchModel)
    {
        // Ensure SearchModel is not null so the view's asp-for bindings work even when user leaves fields empty
        SearchModel = searchModel ?? new InventorySearchModel();

        var allProducts = _productApplication.GetProducts();
        Products = new SelectList(allProducts, "Id", "Name");
        Inventories = _inventoryApplication.Search(SearchModel, WatchDeleted);
        watch = WatchDeleted;
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateInventory
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
    public JsonResult OnPostCreate(CreateInventory command)
    {
        var result = _inventoryApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var product = _inventoryApplication.GetDetails(id);
        product.Products = _productApplication.GetProducts();
        return Partial("Edit", product);
    }

    //[NeedsPermission(ShopPermissions.EditProduct)]
    public JsonResult OnPostEdit(EditInventory command)
    {
        var result = _inventoryApplication.Edit(command);
        return new JsonResult(result);
    }

    public RedirectToPageResult OnGetRemove(long id)
    {
        _inventoryApplication.Remove(id);
        return RedirectToPage();
    }

    public RedirectToPageResult OnGetRestore(long id)
    {
        _inventoryApplication.Restore(id);
        return RedirectToPage("./Index", OnGetDeleted());
    }

    public IActionResult OnGetIncrease(long id)
    {
        var command = new IncreaseInventory
        {
            InventoryId = id,
            OperatorId = _authHelper.CurrentAccountInfo().Id
        };
        return Partial("Increase", command);
    }

    public JsonResult OnPostIncrease(IncreaseInventory command)
    {
        var result = _inventoryApplication.Increase(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetReduce(long id)
    {
        
        var command = new ReduceInventory
        {
            InventoryId = id,
            OperatorId = _authHelper.CurrentAccountInfo().Id
        };
        return Partial("Reduce", command);
    }

    public JsonResult OnPostReduce(ReduceInventory command)
    {
        var result = _inventoryApplication.Reduce(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetLog(long id)
    {
        var log = _inventoryApplication.GetOperationLog(id);
        return Partial("OperationLog", log);
    }
}