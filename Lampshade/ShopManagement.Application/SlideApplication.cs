using _0_Framework.Application;
using ShopManagement.Application.Contracts.SlideAgg;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.SlideAgg;

namespace ShopManagement.Application;

public class SlideApplication : ISlideApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IProductRepository _productRepository;
    private readonly ISlideRepository _slideRepository;

    public SlideApplication(ISlideRepository slideRepository, IFileUploader fileUploader,
        IProductRepository productRepository)
    {
        _slideRepository = slideRepository;
        _fileUploader = fileUploader;
        _productRepository = productRepository;
    }


    public OperationResult Create(CreateSlide command)
    {
        var operationResult = new OperationResult();
        if (_slideRepository.Exists(x => x.Picture == command.Picture.ToString()))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _slideRepository.BeginTran();
        try
        {
            var pictureName = _fileUploader.Upload(command.Picture, "slides");
            var p1 = new Slide(pictureName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title,
                command.BtnText, command.Link, command.BtnText);
            _slideRepository.Create(p1);
        }
        catch (Exception e)
        {
            _slideRepository.Rollback();
            return operationResult.Fail();
        }

        _slideRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditSlide command)
    {
        var operationResult = new OperationResult();
        //if (_slideRepository.Exists(x => x.Picture == command.Picture))
        //    return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        var pictureName = _fileUploader.Upload(command.Picture, "slides");
        _slideRepository.BeginTran();
        try
        {
            var p1 = _slideRepository.Get(command.Id);
            p1.Edit(pictureName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title,
                command.BtnText, command.Link, command.BtnText);
        }
        catch (Exception e)
        {
            _slideRepository.Rollback();
            return operationResult.Fail();
        }

        _slideRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Remove(long id)
    {
        var operationResult = new OperationResult();
        _slideRepository.BeginTran();
        try
        {
            _slideRepository.Get(id).Remove();
        }
        catch (Exception e)
        {
            _slideRepository.Rollback();
            return operationResult.Fail();
        }

        _slideRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _slideRepository.BeginTran();
        try
        {
            _slideRepository.Get(id).Restore();
        }
        catch (Exception e)
        {
            _slideRepository.Rollback();
            return operationResult.Fail();
        }

        _slideRepository.CommitTran();
        return operationResult.Success();
    }

    public EditSlide GetDetails(long id)
    {
        return _slideRepository.GetDetails(id);
    }

    public List<SlideViewModel> GetList()
    {
        return _slideRepository.GetList();
    }
}