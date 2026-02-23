using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductAgg;

namespace ShopManagementDomain.ProductAgg;

public interface IProductRepository : IRepository<long,Product>
{
    EditProduct? GetDetails(long id);

    List<ProductViewModel> Search(ProductSearchModel searchModel, bool showDeleted);

    Product GetProductWithCategory(long id);

    List<ProductViewModel> GetProducts();

    List<EditProduct> GetList();
}