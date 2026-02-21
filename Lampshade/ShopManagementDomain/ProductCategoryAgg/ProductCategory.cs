using _0_Framework.Domain;

namespace ShopManagementDomain.ProductCategoryAgg;

public class ProductCategory : EntityBase<long>
{
    public ProductCategory()
    {
    }

    public ProductCategory(string title, string picture, string pictureAlt, string pictureTitle, string metaDescription,
        string keywords, string slug)
    {
        Title = title;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        MetaDescription = metaDescription;
        Keywords = keywords;
        Slug = slug;
    }

    public string Title { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }

    public string PictureTitle { get; private set; }

    public string MetaDescription { get; private set; }

    public string Keywords { get; private set; }

    public string Slug { get; private set; }

    public void Edit(string title, string picture, string pictureAlt, string pictureTitle, string metaDescription,
        string keywords, string slug)
    {
        Title = title;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        MetaDescription = metaDescription;
        Keywords = keywords;
        Slug = slug;
    }
}