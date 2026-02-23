using _0_Framework.Domain;
using ShopManagementDomain.ProductCategoryAgg;
using ShopManagementDomain.ProductPictureAgg;

namespace ShopManagementDomain.ProductAgg;

public class Product : EntityBase<long>
{
    public Product()
    {
    }

    public Product(string name, string code, string shortDescription,string description, string picture, string pictureAlt, string pictureTitle, string metaDescription, string keywords, string slug, long categoryId)
    {
        Name = name;
        Code = code;
        ShortDescription = shortDescription;
        Description = description;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        CategoryId = categoryId;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
    }
    public string Name { get; private set; }

    public string Code { get; private set; }

    public string ShortDescription { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public string Picture { get; private set; }

    public string PictureAlt { get; private set; }

    public string PictureTitle { get; private set; }

    public string MetaDescription { get; private set; }

    public string Keywords { get; private set; }

    public string Slug { get; private set; }

    public long CategoryId { get; private set; }
    public ProductCategory Category { get; private set; }

    public List<ProductPicture> ProductPictures { get; private set; }

    public void Edit(string name, string code, string shortDescription,string description, string picture, string pictureAlt, string pictureTitle, string metaDescription, string keywords, string slug, long categoryId)
    {
        Name = name;
        Code = code;
        ShortDescription = shortDescription;
        Description = description;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        CategoryId = categoryId;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
    }
}