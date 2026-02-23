using _0_Framework.Domain;
using ShopManagementDomain.ProductAgg;

namespace ShopManagementDomain.ProductPictureAgg;

public class ProductPicture : EntityBase<long>
{
    public ProductPicture(long productId, string picture, string pictureAlt, string pictureTitle)
    {
        ProductId = productId;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
    }

    public long ProductId { get; private set; }
    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }
    public string PictureTitle { get; private set; }
    public Product Product { get; private set; }

    public void Edit(long productId, string picture, string pictureAlt, string pictureTitle)
    {
        ProductId = productId;

        if (!string.IsNullOrWhiteSpace(picture))
            Picture = picture;

        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
    }
}