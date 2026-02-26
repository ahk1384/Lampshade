using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.ProductCategoryAgg;
using ShopManagementDomain.ProductPictureAgg;

namespace ShopManagement.Application;

public class ProductPictureApplication : IProductPictureApplication
{
    private readonly IProductPictureRepository _pictureRepository;
    private readonly IFileUploader _fileUploader;
    private readonly IProductRepository _productRepository;
    public ProductPictureApplication(IProductPictureRepository pictureRepository, IFileUploader fileUploader, IProductRepository productRepository)
    {
        _pictureRepository = pictureRepository;
        _fileUploader = fileUploader;
        _productRepository = productRepository;
    }

    public OperationResult Create(CreateProductPicture command)
    {
        var operationResult = new OperationResult();
        //if (_pictureRepository.Exists(x => x.Picture == command.Picture.ToString()))
        //    return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        var product = _productRepository.GetProductWithCategory(command.ProductId);
        var path = $"{product.Category.Slug}//{product.Slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        _pictureRepository.BeginTran();

        try
        {
            var p1 = new ProductPicture(command.ProductId, picturePath, command.PictureAlt,
                command.PictureTitle);
            _pictureRepository.Create(p1);
        }
        catch (Exception e)
        {
            _pictureRepository.Rollback();
            return operationResult.Fail();
        }

        _pictureRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditProductPicture command)
    {
        var operationResult = new OperationResult();
        var product = _productRepository.GetProductWithCategory(command.ProductId);

        var path = $"{product.Category.Slug}//{product.Slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);
        _pictureRepository.BeginTran();
        try
        {
            var productPicture= _pictureRepository.Get(command.Id);
            productPicture.Edit(command.ProductId, picturePath, command.PictureAlt, command.PictureTitle);
        }
        catch (Exception e)
        {
            _pictureRepository.Rollback();
            return operationResult.Fail();
        }

        _pictureRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Remove(long id)
    {
        var operationResult = new OperationResult();
        _pictureRepository.BeginTran();
        try
        {
            _pictureRepository.Get(id).Remove();
        }
        catch (Exception e)
        {
            _pictureRepository.Rollback();
            return operationResult.Fail();
        }

        _pictureRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _pictureRepository.BeginTran();
        try
        {
            _pictureRepository.Get(id).Restore();
        }
        catch (Exception e)
        {
            _pictureRepository.Rollback();
            return operationResult.Fail();
        }

        _pictureRepository.CommitTran();
        return operationResult.Success();
    }

    public EditProductPicture GetDetails(long id)
    {
        return _pictureRepository.GetDetails(id);
    }

    public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel, bool showDeleted)
    {
        return _pictureRepository.Search(searchModel,showDeleted);
    }
}