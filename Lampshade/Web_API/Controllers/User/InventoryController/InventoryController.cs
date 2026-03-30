using _01_LampshadeQuery.Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers.User.InventoryController;

[ApiController]
[Route("api/Inventory/")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryQuery _inventoryQuery;

    public InventoryController(IInventoryQuery inventoryQuery)
    {
        _inventoryQuery = inventoryQuery;
    }

    [HttpPost("checkStock")]
    public StockStatus CheckStock(IsInStock command)
    {
        return _inventoryQuery.CheckStock(command);
    }
}