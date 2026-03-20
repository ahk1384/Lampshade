namespace _0_Framework.Application.Sms.Models;

public class BulkReceiveModel
{
    public BulkReceiveModel(Guid packId, int[] messageIds, decimal cost)
    {
        PackId = packId;
        MessageIds = messageIds;
        Cost = cost;
    }

    public Guid PackId { get; set; }
    public int[] MessageIds { get; set; }
    public decimal Cost { get; set; }
}