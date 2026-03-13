using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.ProductCategoryAgg;

public interface IProductCategoryApplication
{
    List<ProductCategoryViewModel> GetProductCategories();

    OperationResult Add(CreateProductCategory productCategory);

    OperationResult Edit(EditProductCategory productCategory);

    OperationResult Remove(long id);

    OperationResult Restore(long id);

    List<EditProductCategory> GetList();

    EditProductCategory GetDetails(long id);

    List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel, bool showDeleted = false);
}