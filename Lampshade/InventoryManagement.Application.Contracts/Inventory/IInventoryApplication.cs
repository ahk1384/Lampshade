using _0_Framework.Application;

namespace InventoryManagement.Application.Contracts.Inventory;

public interface IInventoryApplication
{
    OperationResult Create(CreateInventory command);
    OperationResult Edit(EditInventory command);
    OperationResult Increase(IncreaseInventory command);
    OperationResult Reduce(ReduceInventory command);
    OperationResult Reduce(List<ReduceInventory> command);
    OperationResult Remove(long id);
    OperationResult Restore(long id);
    EditInventory GetDetails(long id);
    List<InventoryViewModel> Search(InventorySearchModel searchModel, bool watchDeleted);
    List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
}