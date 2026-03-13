using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductPicture;

namespace ShopManagementDomain.ProductPictureAgg;

public interface IProductPictureRepository : IRepository<long, ProductPicture>
{
    EditProductPicture GetDetails(long id);
    ProductPicture GetWithProductAndCategory(long id);
    List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel, bool showDeleted = false);
}