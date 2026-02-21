namespace ShopManagement.Application.Contracts.ProductCategory;

public class CreateProductCategory
{
    public string Title { get; set; }

    public string Description { get; set; } = string.Empty;

    public string CreationDate { get; set; }

    public string Picture { get; set; }
    public string PictureAlt { get; set; }

    public string PictureTitle { get; set; }

    public string MetaDescription { get; set; }

    public string Keywords { get; set; }

    public string Slug { get; set; }
}