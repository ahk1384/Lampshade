namespace ShopManagement.Application.Contracts.ProductCategory;

public class CreateProductCategory
{
    public string Title { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public string CreationDate { get; set; }

    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }

    public string PictureTitle { get; private set; }

    public string MetaDezcription { get; private set; }

    public string Keywords { get; private set; }

    public string Slug { get; private set; }

}