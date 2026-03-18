using _0_Framework.Domain;

namespace ShopManagementDomain.OrderAgg;

public class Order : EntityBase<long>
{
    public Order(long accountId, int paymentMethod, double totalAmount, double discountAmount, double payAmount)
    {
        AccountId = accountId;
        TotalAmount = totalAmount;
        DiscountAmount = discountAmount;
        PayAmount = payAmount;
        PaymentMethod = paymentMethod;
        IsPaid = false;
        IsCanceled = false;
        RefId = 0;
        IssueTrackingNo = "100";
        Items = new List<OrderItem>();
    }

    protected Order()
    {
    }

    public long AccountId { get; }
    public int PaymentMethod { get; set; }
    public double TotalAmount { get; set; }
    public double DiscountAmount { get; set; }
    public double PayAmount { get; set; }
    public bool IsPaid { get; private set; }
    public bool IsCanceled { get; private set; }
    public string IssueTrackingNo { get; private set; }
    public long RefId { get; private set; }
    public List<OrderItem> Items { get; }

    public void PaymentSucceeded(long refId)
    {
        IsPaid = true;

        if (refId != 0)
            RefId = refId;
    }

    public void SetPaymenMethod(int paymentMethod)
    {
        PaymentMethod = paymentMethod;
    }
    public void Cancel()
    {
        IsCanceled = true;
    }

    public void SetIssueTrackingNo(string number)
    {
        IssueTrackingNo = number;
    }

    public void AddItem(OrderItem item)
    {
        Items.Add(item);
    }
}