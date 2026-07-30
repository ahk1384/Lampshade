namespace InventoryManagement.Application.Contracts.Inventory;

public class IncreaseInventory
{
    public IncreaseInventory(long inventoryId, long operatorId, long count, string description)
    {
        InventoryId = inventoryId;
        OperatorId = operatorId;
        Count = count;
        Description = description;
    }

    public IncreaseInventory(long inventoryId, long operatorId)
    {
        InventoryId = inventoryId;
        OperatorId = operatorId;
    }

    public IncreaseInventory()
    {
    }


    public long InventoryId { get; set; }

    public long OperatorId { get; set; }
    public long Count { get; set; }
    public string Description { get; set; }
}