using _01_LampshadeQuery.Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryQuery _inventoryQuery;

        public InventoryController(IInventoryQuery inventoryQuery)
        {
            _inventoryQuery = inventoryQuery;
        }

        [HttpPost]
        public StockStatus CheckStatus(IsInStock command)
        {
            return _inventoryQuery.CheckStock(command);
        }
    }
}
