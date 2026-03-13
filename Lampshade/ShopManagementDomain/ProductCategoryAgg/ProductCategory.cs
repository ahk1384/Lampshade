using _0_Framework.Domain;
using ShopManagementDomain.ProductAgg;

namespace ShopManagementDomain.ProductCategoryAgg;

public class ProductCategory : EntityBase<long>
{
    public ProductCategory()
    {
        Products = new List<Product>();
    }

    public ProductCategory(string title, string picture, string description, string pictureAlt, string pictureTitle,
        string metaDescription,
        string keywords, string slug)
    {
        Title = title;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Description = description;
        MetaDescription = metaDescription;
        Keywords = keywords;
        Slug = slug;
    }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }

    public string PictureTitle { get; private set; }

    public string MetaDescription { get; private set; }

    public string Keywords { get; private set; }

    public string Slug { get; private set; }

    public List<Product> Products { get; }

    public void Edit(string title, string picture, string description, string pictureAlt, string pictureTitle,
        string metaDescription,
        string keywords, string slug)
    {
        Title = title;
        if (!string.IsNullOrWhiteSpace(picture))
            Picture = picture;
        Description = description;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        MetaDescription = metaDescription;
        Keywords = keywords;
        Slug = slug;
    }
}