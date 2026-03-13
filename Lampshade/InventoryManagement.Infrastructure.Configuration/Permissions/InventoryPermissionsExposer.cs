using _0_Framework.Infrastructure;

namespace InventoryManagement.Infrastructure.Configuration.Permissions;

public class InventoryPermissionsExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "inventory", new List<PermissionDto>
                {
                    new(InventoryPermissions.InventoryList, "Inventory List"),
                    new(InventoryPermissions.CreateInventory, "CreateInventory"),
                    new(InventoryPermissions.EditInventory, "EditInventory"),
                    new(InventoryPermissions.SearchInventory, "SearchInventory"),
                    new(InventoryPermissions.IncreaseInventory, "IncreaseInventory"),
                    new(InventoryPermissions.ReduceInventory, "ReduceInventory"),
                    new(InventoryPermissions.RemoveAndRestoreInventory, "RemoveAndRestoreInventory"),
                    new(InventoryPermissions.OperationLog, "OperationLog")
                }
            }
        };
    }
}