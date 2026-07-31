using _0_Framework.Application;
using ClosedXML.Excel;
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
    private readonly IAuthHelper _authHelper;
    private readonly IInventoryApplication _inventoryApplication;
    private readonly IProductApplication _productApplication;
    public List<InventoryViewModel> Inventories;
    public SelectList Products;
    public InventorySearchModel SearchModel;
    public bool watch;

    public IndexModel(IInventoryApplication inventoryApplication, IProductApplication productApplication,
        IAuthHelper authHelper)
    {
        _productApplication = productApplication;
        _authHelper = authHelper;
        _inventoryApplication = inventoryApplication;
    }

    [TempData] public string Message { get; set; }

    public void OnGet(InventorySearchModel searchModel)
    {
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
        var command = new IncreaseInventory(id, _authHelper.CurrentAccountInfo().Id);
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

    public IActionResult OnGetExportExcel(InventorySearchModel searchModel)
    {
        var data = _inventoryApplication.Search(searchModel, WatchDeleted);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("All Inventories");

        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "ProductName";
        worksheet.Cell(1, 3).Value = "CreationDate";
        worksheet.Cell(1, 4).Value = "UnitPrice";
        worksheet.Cell(1, 5).Value = "CurrentCount";

        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;

        for (int i = 0; i < data.Count; i++)
        {
            var row = i + 2;
            worksheet.Cell(row, 1).Value = data[i].Id;
            worksheet.Cell(row, 2).Value = data[i].Product;
            worksheet.Cell(row, 3).Value = data[i].CreationDate;
            worksheet.Cell(row, 3).Style.DateFormat.Format = "yyyy-mm-dd";
            worksheet.Cell(row, 4).Value = data[i].UnitPrice;
            worksheet.Cell(row, 5).Value = data[i].CurrentCount;
        }

        worksheet.Columns().AdjustToContents();
        for (int i = 0; i < data.Count; i++)
        {
            var worksheet2 = workbook.Worksheets.Add(data[i].Product);
            var log = _inventoryApplication.GetOperationLog(data[i].Id);
            worksheet2.Cell(1, 1).Value = "OrderId";
            worksheet2.Cell(1, 2).Value = "Count";
            worksheet2.Cell(1, 3).Value = "OperationDate";
            worksheet2.Cell(1, 4).Value = "OperationType";
            worksheet2.Cell(1, 5).Value = "CurrentCount";
            worksheet2.Cell(1, 6).Value = "OperatorId";
            worksheet2.Cell(1, 7).Value = "Description";
            var headerRow2 = worksheet2.Row(1);
            headerRow2.Style.Font.Bold = true;
            for (int j = 0; j < log.Count; j++)
            {
                var row = j + 2;
                worksheet2.Cell(row, 1).Value = log[j].OrderId;
                worksheet2.Cell(row, 2).Value = log[j].Count;
                worksheet2.Cell(row, 3).Value = log[j].OperationDate;
                worksheet2.Cell(row, 3).Style.DateFormat.Format = "yyyy-mm-dd";
                worksheet2.Cell(row, 4).Value = log[j].Operation ? "Increase" : "Decrease";
                worksheet2.Cell(row, 5).Value = log[j].CurrentCount;
                worksheet2.Cell(row, 6).Value = log[j].OperatorId;
                worksheet2.Cell(row, 7).Value = log[j].Description;
            }

            worksheet2.Columns().AdjustToContents();
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }
}