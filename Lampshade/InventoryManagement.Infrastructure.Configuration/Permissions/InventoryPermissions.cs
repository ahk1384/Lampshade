namespace InventoryManagement.Infrastructure.Configuration.Permissions;

public class InventoryPermissions
{
    public const int BaseInventory = 2000;
    public const int CreateInventory = BaseInventory+01;
    public const int EditInventory = BaseInventory+02;
    public const int RemoveAndRestoreInventory = BaseInventory+03;
    public const int IncreaseInventory = BaseInventory+04;
    public const int ReduceInventory = BaseInventory+05;
    public const int SearchInventory = BaseInventory+06;
    public const int OperationLog = BaseInventory+07;
    public const int InventoryList = BaseInventory + 08;
}