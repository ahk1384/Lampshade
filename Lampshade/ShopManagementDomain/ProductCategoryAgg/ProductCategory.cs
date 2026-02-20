using _0_Framework.Application;
using _0_Framework.Domain;

namespace ShopManagementDomain.ProductCategoryAgg;

public class ProductCategory : EntityBase<long>
{
    public string  Title { get; private set; }

    public string Description { get; private set; }= string.Empty;

    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }

    public string PictureTitle { get; private set; }

    public string MetaDezcription { get; private set; }

    public string Keywords { get; private set; }

    public string Slug { get; private set; }

    public ProductCategory()
    {
        
    }

    public ProductCategory(string title, string picture, string pictureAlt, string pictureTitle, string metaDezcription, string keywords, string slug)
    {
        Title = title;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        MetaDezcription = metaDezcription;
        Keywords = keywords;
        Slug = slug;
    }
}