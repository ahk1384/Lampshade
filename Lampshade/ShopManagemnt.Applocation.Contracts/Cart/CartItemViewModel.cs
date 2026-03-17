namespace ShopManagement.Application.Contracts.Cart;

public class CartItemViewModel
{
    public CartItemViewModel(long productId, string name, double unitPrice, string picture, int count,
        double totalItemPrice, bool isInStock, int discountRate, string productSlug, double discountAmount,
        double itemPayAmount)
    {
        ProductId = productId;
        Name = name;
        UnitPrice = unitPrice;
        Picture = picture;
        Count = count;
        TotalItemPrice = totalItemPrice;
        IsInStock = isInStock;
        DiscountRate = discountRate;
        ProductSlug = productSlug;
        DiscountAmount = discountAmount;
        ItemPayAmount = itemPayAmount;
    }

    public CartItemViewModel(long productId, string name, double unitPrice, string picture, int count, bool isInStock,
        int discountRate, string productSlug)
    {
        Name = name;
        UnitPrice = unitPrice;
        ProductId = productId;
        Picture = picture;
        Count = count;
        IsInStock = isInStock;
        DiscountRate = discountRate;
        ProductSlug = productSlug;
        TotalItemPrice = count * unitPrice;
        DiscountAmount = count * unitPrice * DiscountRate / 100;
        ItemPayAmount = TotalItemPrice - DiscountAmount;
    }

    protected CartItemViewModel()
    {
    }

    public long ProductId { get; set; }
    public string Name { get; set; }
    public double UnitPrice { get; set; }
    public string Picture { get; set; }
    public int Count { get; set; }
    public double TotalItemPrice { get; set; }
    public bool IsInStock { get; set; }
    public int DiscountRate { get; set; }

    public string ProductSlug { get; set; }
    public double DiscountAmount { get; set; }
    public double ItemPayAmount { get; set; }

    public void CalculateTotalItemPrice()
    {
        TotalItemPrice = UnitPrice * Count;
    }
}