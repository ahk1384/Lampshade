using ShopManagementDomain.OrderAgg;

namespace ShopManagementDomain.Services;

public interface IShopInventoryAcl
{
    bool ReduceFromInventory(List<OrderItem> items);
    bool IncreaseFromInventory(List<OrderItem> items);
}