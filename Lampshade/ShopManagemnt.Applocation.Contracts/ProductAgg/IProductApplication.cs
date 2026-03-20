using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.ProductAgg;

public interface IProductApplication
{
    OperationResult Add(CreateProduct product);

    OperationResult Edit(EditProduct productCategory);

    OperationResult Remove(long id);

    OperationResult Restore(long id);

    EditProduct? GetDetails(long id);

    List<ProductViewModel> Search(ProductSearchModel searchModel, bool showDeleted = false);

    List<ProductViewModel> GetProducts();
}