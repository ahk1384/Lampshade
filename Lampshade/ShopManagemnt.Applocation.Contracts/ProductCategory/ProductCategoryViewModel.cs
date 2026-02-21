namespace ShopManagement.Application.Contracts.ProductCategory;

public class ProductCategoryViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Picture { get; set; }

    public string CreationDate { get; set; }
}