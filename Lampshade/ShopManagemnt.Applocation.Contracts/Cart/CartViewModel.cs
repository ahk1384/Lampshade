namespace ShopManagement.Application.Contracts.Cart;

public class CartViewModel
{
    public long CartId { get; set; }
    public long AccountId { get; set; }
    public double TotalAmount { get; set; }
    public double DiscountAmount { get; set; }
    public double PayAmount { get; set; }
    public int PaymentMethod { get; set; }
    public List<CartItemViewModel> Items { get; set; } = new();

    public void SetPaymentMethod(int paymentMethod)
    {
        PaymentMethod = paymentMethod;
    }

    public void Add(CartItemViewModel cartItem)
    {
        TotalAmount += cartItem.TotalItemPrice;
        DiscountAmount += cartItem.DiscountAmount;
        PayAmount += cartItem.ItemPayAmount;
        Items.Add(cartItem);
    }
}