using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using SM.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repositories;

public class InventoryRepository : BaseRepository<long, Inventory>, IInventoryRepository
{
    private readonly AccountContext _accountContext;
    private readonly InventoryContext _context;
    private readonly ShopContext _shopContext;

    public InventoryRepository(InventoryContext context, ShopContext shopContext, AccountContext accountContext) :
        base(context)
    {
        _context = context;
        _shopContext = shopContext;
        _accountContext = accountContext;
    }

    public EditInventory GetDetails(long id)
    {
        return _context.Inventories.Select(x => new EditInventory
        {
            Id = x.Id,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        var inventory = _context.Inventories.FirstOrDefault(x => x.Id == inventoryId);
        var operations = inventory.Operations.Select(x => new InventoryOperationViewModel
        {
            Id = x.Id,
            Count = x.Count,
            CurrentCount = x.CurrentCount,
            Description = x.Description,
            Operation = x.Operation,
            OperationDate = x.OperationDate.ToFarsiFull(),
            OperatorId = x.OperatorId,
            OrderId = x.OrderId,
            Operator = _accountContext.Accounts.FirstOrDefault(w => w.Id == x.OperatorId).Username
        });
        return operations.OrderByDescending(x => x.Id).ToList();
    }

    public long GetInventoryId(long productId)
    {
        return _context.Inventories.FirstOrDefault(x => x.ProductId == productId).Id;
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel, bool watchDeleted)
    {
        var res = watchDeleted
            ? _context.Inventories.Where(x => x.IsDeleted)
            : _context.Inventories.Where(x => !x.IsDeleted);
        var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();
        var query = res.Select(x => new InventoryViewModel
        {
            Id = x.Id,
            UnitPrice = x.UnitPrice,
            InStock = x.InStock,
            ProductId = x.ProductId,
            CurrentCount = x.CalculateCurrentCount(),
            CreationDate = x.CreationDate.ToFarsi()
        });

        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        query = query.Where(x => x.InStock != searchModel.InStock);

        var inventory = query.OrderBy(x => x.Id).ToList();

        inventory.ForEach(item =>
            item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name);

        return inventory;
    }
}