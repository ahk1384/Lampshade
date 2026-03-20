namespace _0_Framework.Application.Sms.Models;

public class BulkReceiveModel
{
    public Guid PackId { get; set; }
    public int[] MessageIds { get; set; }
    public Decimal Cost { get; set; }

    public BulkReceiveModel(Guid packId, int[] messageIds, decimal cost)
    {
        PackId = packId;
        MessageIds = messageIds;
        Cost = cost;
    }
}