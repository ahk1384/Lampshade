namespace ShopManagementDomain.CartAgg;

public class CartItem
{
    public CartItem(long cartId, long productId, string name, double unitPrice, string picture, int count,
        bool isInStock, int discountRate, string productSlug)
    {
        ProductId = productId;
        CartId = cartId;
        Name = name;
        UnitPrice = unitPrice;
        Picture = picture;
        Count = count;
        IsInStock = isInStock;
        DiscountRate = discountRate;
        ProductSlug = productSlug;

        DiscountAmount = count * unitPrice * discountRate / 100;
        TotalItemPrice = UnitPrice * Count;
        ItemPayAmount = TotalItemPrice - DiscountAmount;
    }

    protected CartItem(string productSlug)
    {
        ProductSlug = productSlug;
    }

    public long Id { get; private set; }

    public long ProductId { get; private set; }
    public string Name { get; private set; }
    public double UnitPrice { get; }
    public string Picture { get; private set; }
    public int Count { get; private set; }
    public double TotalItemPrice { get; private set; }
    public bool IsInStock { get; private set; }
    public int DiscountRate { get; private set; }
    public string ProductSlug { get; private set; }
    public double DiscountAmount { get; private set; }
    public double ItemPayAmount { get; private set; }
    public long CartId { get; private set; }
    public Cart Cart { get; private set; }

    public void AddCount(int count)
    {
        Count += count;
    }

    public void SetCount(int count)
    {
        Count = count;
    }

    public void CalculateTotalItemPrice()
    {
        TotalItemPrice = UnitPrice * Count;
        DiscountAmount = TotalItemPrice - DiscountRate * Count * UnitPrice / 100;
        ItemPayAmount = TotalItemPrice - DiscountAmount;
    }

    public void SetDiscountRate(int discountRate)
    {
        DiscountRate = discountRate;
    }
}