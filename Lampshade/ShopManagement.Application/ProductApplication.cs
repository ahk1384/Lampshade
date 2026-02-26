using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductAgg;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.ProductCategoryAgg;
using System.Globalization;

namespace ShopManagement.Application;

public class ProductApplication : IProductApplication
{
    private readonly IProductRepository _productRepository;
    private readonly IFileUploader _fileUploader;

    public ProductApplication(IProductRepository productRepository, IFileUploader fileUploader)
    {
        _productRepository = productRepository;
        _fileUploader = fileUploader;
    }

    public OperationResult Add(CreateProduct product)
    {
        var operationResult = new OperationResult();
        if (_productRepository.Exists(x => x.Name == product.Name))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _productRepository.BeginTran();
        try
        {
            var slug = product.Slug.Slugify();

            var picturePath = $"{product.Slug}";
            var pictureName = _fileUploader.Upload(product.Picture, picturePath);
            var p1 = new Product(product.Name, product.Code, product.ShortDescription, product.Description, pictureName,
                product.PictureAlt, product.PictureTitle, product.MetaDescription, product.Keywords, product.Slug,
                product.CategoryId);
            _productRepository.Create(p1);
        }
        catch (Exception e)
        {
            _productRepository.Rollback();
            return operationResult.Fail();
        }

        _productRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditProduct product)
    {
        var operationResult = new OperationResult();
        _productRepository.BeginTran();
        try
        {
            var p1 = _productRepository.Get(product.Id);
            var slug = product.Slug.Slugify();

            var picturePath = $"{product.Slug}";
            var pictureName = _fileUploader.Upload(product.Picture, picturePath);
            p1.Edit(product.Name, product.Code, product.ShortDescription, product.Description, pictureName,
                product.PictureAlt, product.PictureTitle, product.MetaDescription, product.Keywords, product.Slug,
                product.CategoryId);
        }
        catch (Exception e)
        {
            _productRepository.Rollback();
            return operationResult.Fail();
        }

        _productRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Remove(long id)
    {
        var operationResult = new OperationResult();
        _productRepository.BeginTran();
        try
        {
            _productRepository.Get(id).Remove();
        }
        catch (Exception e)
        {
            _productRepository.Rollback();
            return operationResult.Fail();
        }

        _productRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _productRepository.BeginTran();
        try
        {
            _productRepository.Get(id).Restore();
        }
        catch (Exception e)
        {
            _productRepository.Rollback();
            return operationResult.Fail();
        }

        _productRepository.CommitTran();
        return operationResult.Success();
    }

    public EditProduct? GetDetails(long id)
    {
        return _productRepository.GetDetails(id);
    }

    public List<ProductViewModel> Search(ProductSearchModel searchModel, bool showDeleted = false)
    {
        return _productRepository.Search(searchModel, showDeleted);
    }

    public List<ProductViewModel> GetProducts()
    {
        return _productRepository.GetProducts();
    }
}