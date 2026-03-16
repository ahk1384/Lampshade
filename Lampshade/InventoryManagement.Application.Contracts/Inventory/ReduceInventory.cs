namespace InventoryManagement.Application.Contracts.Inventory;

public class ReduceInventory
{
    public ReduceInventory()
    {
    }

    public ReduceInventory(long productId, long count, string description, long orderId,long operatorId)
    {
        ProductId = productId;
        Count = count;
        Description = description;
        OrderId = orderId;
        OperatorId = operatorId;
    }

    public ReduceInventory(long inventoryId, long productId, long count, string description, long orderId, long operatorId)
    {
        InventoryId = inventoryId;
        ProductId = productId;
        Count = count;
        Description = description;
        OrderId = orderId;
        OperatorId = operatorId;
    }

    public long InventoryId { get; set; }
    public long ProductId { get; set; }
    public long Count { get; set; }
    public string Description { get; set; }
    public long OrderId { get; set; }
    
    public long OperatorId { get; set; }
}