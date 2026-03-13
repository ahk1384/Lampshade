using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contracts.Inventory;

namespace InventoryManagement.Domain.InventoryAgg;

public interface IInventoryRepository : IRepository<long, Inventory>
{
    EditInventory GetDetails(long id);
    List<InventoryViewModel> Search(InventorySearchModel searchModel, bool watchDeleted);
    List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
}