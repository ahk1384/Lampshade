using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application;

public class InventoryApplication : IInventoryApplication
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryApplication(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public OperationResult Create(CreateInventory command)
    {
        var operationResult = new OperationResult();
        _inventoryRepository.BeginTran();
        if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        try
        {
            var discount = new Inventory(command.ProductId, command.UnitPrice);
            _inventoryRepository.Create(discount);
        }
        catch (Exception e)
        {
            _inventoryRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _inventoryRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditInventory command)
    {
        var operationResult = new OperationResult();
        _inventoryRepository.BeginTran();
        try
        {
            var inventory = _inventoryRepository.Get(command.Id);
            if (inventory == null)
                return operationResult.Fail(ApplicationMessages.RecordNotFound);

            if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id))
                return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
            inventory.Edit(command.ProductId, command.UnitPrice);
        }
        catch (Exception e)
        {
            _inventoryRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _inventoryRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Increase(IncreaseInventory command)
    {
        var operationResult = new OperationResult();
        _inventoryRepository.BeginTran();
        try
        {
            var inventory = _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
                return operationResult.Fail(ApplicationMessages.RecordNotFound);
            const long operatorId = 1;
            inventory.Increase(command.Count, operatorId, command.Description);
        }
        catch (Exception e)
        {
            _inventoryRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _inventoryRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Reduce(ReduceInventory command)
    {
        var operationResult = new OperationResult();
        _inventoryRepository.BeginTran();
        try
        {
            var inventory = _inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
                return operationResult.Fail(ApplicationMessages.RecordNotFound);
            const long operatorId = 1;
            inventory.Reduce(command.Count, operatorId, command.Description);
        }
        catch (Exception e)
        {
            _inventoryRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _inventoryRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Reduce(List<ReduceInventory> command)
    {
        var operationResult = new OperationResult();

        foreach (var inventory in command)
        {
            operationResult = Reduce(inventory);
            if (!operationResult.IsSuccess) return operationResult;
        }

        return operationResult.Success();
    }

    public OperationResult Remove(long id)
    {
        var operationResult = new OperationResult();
        _inventoryRepository.BeginTran();
        try
        {
            var inventory = _inventoryRepository.Get(id);
            if (inventory == null)
                return operationResult.Fail(ApplicationMessages.RecordNotFound);
            inventory.Remove();
        }
        catch (Exception e)
        {
            _inventoryRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _inventoryRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _inventoryRepository.BeginTran();
        try
        {
            var inventory = _inventoryRepository.Get(id);
            if (inventory == null)
                return operationResult.Fail(ApplicationMessages.RecordNotFound);
            inventory.Restore();
        }
        catch (Exception e)
        {
            _inventoryRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _inventoryRepository.CommitTran();
        return operationResult.Success();
    }

    public EditInventory GetDetails(long id)
    {
        return _inventoryRepository.GetDetails(id);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel, bool watchDeleted)
    {
        return _inventoryRepository.Search(searchModel, watchDeleted);
    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        return _inventoryRepository.GetOperationLog(inventoryId);
    }
}