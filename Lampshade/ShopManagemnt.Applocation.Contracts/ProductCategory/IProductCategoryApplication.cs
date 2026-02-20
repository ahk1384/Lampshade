using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.ProductCategory;

public interface IProductCategoryApplication
{
    ProductCategoryViewModel Get(long id);

    List<ProductCategoryViewModel> GetAll();

    OperationResult Add(CreateProductCategory productCategory);

    OperationResult Edit(EditProductCategory productCategory);

    OperationResult Remove(long id);

    OperationResult Restore(long id);
}