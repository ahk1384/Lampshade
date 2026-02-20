using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagementDomain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication: IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _repository;

        public ProductCategoryApplication(IProductCategoryRepository repository)
        {
            _repository = repository;
        }

        public ProductCategoryViewModel Get(long id)
        {
            throw new NotImplementedException();
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public OperationResult Add(CreateProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        public OperationResult Edit(EditProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        public OperationResult Remove(long id)
        {
            throw new NotImplementedException();
        }

        public OperationResult Restore(long id)
        {
            throw new NotImplementedException();
        }
    }
}
