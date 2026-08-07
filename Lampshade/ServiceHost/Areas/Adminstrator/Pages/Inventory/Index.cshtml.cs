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
    private readonly IAuthHelper _authHelper;
    private readonly IInventoryApplication _inventoryApplication;
    private readonly IProductApplication _productApplication;
    public ExportExcel Exporter;
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
        Exporter = new ExportExcel();
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

    public IActionResult OnGetExportExcelAll(InventorySearchModel searchModel)
    {
        var inventories = _inventoryApplication.Search(searchModel, WatchDeleted);
        List<List<InventoryOperationViewModel>> operations =
            inventories.Select(inventory => _inventoryApplication.GetOperationLog(inventory.Id)).ToList();
        var formtedData = formatter(inventories, operations);
        var resultFile = Exporter.ExportExcelResult(formtedData);
        return File(
            resultFile,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        );
    }

    public IActionResult OnGetExportExcel(int id)
    {
        List<InventoryOperationViewModel> operations =
            _inventoryApplication.GetOperationLog(id);
        var name = _productApplication.GetDetails(_inventoryApplication.GetDetails(id).ProductId).Name;
        var formtedData = formatter(name, operations);
        var resultFile = Exporter.ExportExcelResult(formtedData);
        return File(
            resultFile,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        );
    }

    private List<ExcelTable> formatter(List<InventoryViewModel> inventories,
        List<List<InventoryOperationViewModel>> operations)
    {
        List<ExcelTable> Table = new List<ExcelTable>();
        var columns = new List<string>() { "Id", "ProductName", "CreationDate", "UnitPrice", "CurrentCount" };
        var rows = new List<ExcelTable.Row>();
        for (int i = 0; i < inventories.Count; i++)
        {
            var item3 = new ExcelTable.Row();
            item3.row.Add(inventories[i].Id.ToString());
            item3.row.Add(inventories[i].Product);
            item3.row.Add(inventories[i].CreationDate);
            item3.row.Add(inventories[i].UnitPrice.ToString());
            item3.row.Add(inventories[i].CurrentCount.ToString());
            rows.Add(item3);
        }

        var item = new ExcelTable("Inventories", rows, columns, 3);
        Table.Add(item);
        for (int z = 0; z < operations.Count; z++)
        {
            var coluns = new List<string>()
            {
                "Order Id", "Count", "Operation Date", "Operation Type", "CurrentCount", "Operator Id", "Description"
            };
            var ros = new List<ExcelTable.Row>();
            for (int j = 0; j < operations[z].Count; j++)
            {
                // for (int i = 0; i < operations[z][j].Count; i++)
                // {
                var item3 = new ExcelTable.Row();
                item3.row.Add(operations[z][j].OrderId.ToString());
                item3.row.Add(operations[z][j].Count.ToString());
                item3.row.Add(operations[z][j].OperationDate);
                item3.row.Add(operations[z][j].Operation ? "Increase" : "Decrease");
                item3.row.Add(operations[z][j].CurrentCount.ToString());
                item3.row.Add(operations[z][j].OperatorId.ToString());
                item3.row.Add(operations[z][j].Description);
                ros.Add(item3);
                // }
            }

            var item2 = new ExcelTable(inventories[z].Product, ros, coluns, 3);
            Table.Add(item2);
        }

        return Table;
    }

    private List<ExcelTable> formatter(
        string name, List<InventoryOperationViewModel> operations)
    {
        List<ExcelTable> Table = new List<ExcelTable>();

        var coluns = new List<string>()
        {
            "Order Id", "Count", "Operation Date", "Operation Type", "CurrentCount", "Operator Id", "Description"
        };
        var ros = new List<ExcelTable.Row>();
        for (int j = 0; j < operations.Count; j++)
        {
            // for (int i = 0; i < operations[z][j].Count; i++)
            // {
            var item3 = new ExcelTable.Row();
            item3.row.Add(operations[j].OrderId.ToString());
            item3.row.Add(operations[j].Count.ToString());
            item3.row.Add(operations[j].OperationDate);
            item3.row.Add(operations[j].Operation ? "Increase" : "Decrease");
            item3.row.Add(operations[j].CurrentCount.ToString());
            item3.row.Add(operations[j].OperatorId.ToString());
            item3.row.Add(operations[j].Description);
            ros.Add(item3);
            // }
        }

        var item2 = new ExcelTable(name, ros, coluns, 3);
        Table.Add(item2);

        return Table;
    }
}