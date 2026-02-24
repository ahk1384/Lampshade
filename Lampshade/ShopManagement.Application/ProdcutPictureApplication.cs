using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.ProductPictureAgg;

namespace ShopManagement.Application;

public class ProductPictureApplication : IProductPictureApplication
{
    private readonly IProductPictureRepository _pictureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductPictureApplication(IProductPictureRepository pictureRepository, IUnitOfWork unitOfWork)
    {
        _pictureRepository = pictureRepository;
        _unitOfWork = unitOfWork;
    }

    public OperationResult Create(CreateProductPicture command)
    {
        var operationResult = new OperationResult();
        if (_pictureRepository.Exists(x => x.Picture == command.Picture.ToString()))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        var pictureProduct = new ProductPicture(command.ProductId, command.Picture.ToString(), command.PictureAlt,
            command.PictureTitle);
        _unitOfWork.BeginTran();
        try
        {
            var p1 = new ProductPicture(pictureProduct.ProductId, pictureProduct.Picture.ToString(), pictureProduct.PictureAlt,
                pictureProduct.PictureTitle);
            _pictureRepository.Create(p1);
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditProductPicture command)
    {
        var operationResult = new OperationResult();
        if (_pictureRepository.Exists(x => x.Picture == command.Picture.ToString()))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _unitOfWork.BeginTran();
        try
        {
            var productPicture= _pictureRepository.Get(command.Id);
            productPicture.Edit(command.ProductId, command.Picture.ToString(), command.PictureAlt, command.PictureTitle);
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Remove(long id)
    {
        var operationResult = new OperationResult();
        _unitOfWork.BeginTran();
        try
        {
            _pictureRepository.Get(id).Remove();
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _unitOfWork.BeginTran();
        try
        {
            _pictureRepository.Get(id).Restore();
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
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