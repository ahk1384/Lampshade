using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategoryAgg;

namespace ShopManagementDomain.ProductCategoryAgg;

public interface IProductCategoryRepository : IRepository<long, ProductCategory>
{
    EditProductCategory? GetDetails(long id);

    List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);

    List<EditProductCategory> GetList();
}