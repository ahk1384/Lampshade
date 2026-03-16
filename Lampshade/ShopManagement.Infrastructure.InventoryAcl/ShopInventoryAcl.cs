using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using ShopManagement.Domain.Services;
using ShopManagementDomain.OrderAgg;
using ShopManagementDomain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl;

public class ShopInventoryAcl : IShopInventoryAcl
{
    private readonly IInventoryApplication _inventoryApplication;
    private readonly IAuthHelper _authHelper;

    public ShopInventoryAcl(IInventoryApplication inventoryApplication, IAuthHelper authHelper)
    {
        _inventoryApplication = inventoryApplication;
        _authHelper = authHelper;
    }

    public bool ReduceFromInventory(List<OrderItem> items)
    {
        
        var command = items.Select(orderItem =>
                new ReduceInventory(_inventoryApplication.GetInventoryId(orderItem.ProductId),orderItem.ProductId, orderItem.Count, "خرید مشتری", orderItem.OrderId,_authHelper.CurrentAccountInfo().Id))
            .ToList();
        
        return _inventoryApplication.Reduce(command).IsSuccess;
    }
}