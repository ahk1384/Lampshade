using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using ShopManagementDomain.OrderAgg;
using ShopManagementDomain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl;

public class ShopInventoryAcl : IShopInventoryAcl
{
    private readonly IAuthHelper _authHelper;
    private readonly IInventoryApplication _inventoryApplication;

    public ShopInventoryAcl(IInventoryApplication inventoryApplication, IAuthHelper authHelper)
    {
        _inventoryApplication = inventoryApplication;
        _authHelper = authHelper;
    }

    public bool ReduceFromInventory(List<OrderItem> items)
    {
        var command = items.Select(orderItem =>
                new ReduceInventory(_inventoryApplication.GetInventoryId(orderItem.ProductId), orderItem.ProductId,
                    orderItem.Count, $"خرید مشتری با شماره پیگیری {orderItem.Order.IssueTrackingNo}", orderItem.OrderId,
                    _authHelper.CurrentAccountInfo().Id))
            .ToList();

        return _inventoryApplication.Reduce(command).IsSuccess;
    }

    public bool IncreaseFromInventory(List<OrderItem> items)
    {
        var command = items.Select(item => new IncreaseInventory(_inventoryApplication.GetInventoryId(item.ProductId),
            _authHelper.CurrentAccountInfo().Id, item.Count,
            $"Cancel order with Issue TrackingNumber ({item.Order.IssueTrackingNo}) and Return to Inventory")).ToList();
        return _inventoryApplication.Increase(command).IsSuccess;
    }
}