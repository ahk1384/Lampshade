using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Order;

namespace ShopManagementDomain.CartAgg;

public class Cart : EntityBase<long>
{
    public Cart(long accountId)
    {
        AccountId = accountId;
        TotalAmount =0;
        DiscountAmount = 0;
        PayAmount = 0;
        PaymentMethod = 1;
        Items = new List<CartItem>();
    }

    protected Cart()
    {
    }

    public void Add(CartItem item)
    {
        Items.Add(item);
    }
    public long AccountId { get;private set; }
    public double TotalAmount { get; private set; }
    public double DiscountAmount { get; private set; }
    public double PayAmount { get;private set; }
    public int PaymentMethod {get; private set; }
    public List<CartItem> Items { get;private set; }
    public void SetPaymentMethod(int methodId)
    {
        PaymentMethod = methodId;
    }
    
}