using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.SlideAgg;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.SlideAgg;
using System.Collections.Generic;

namespace ShopManagement.Application;

public class SlideApplication : ISlideApplication
{
    //private readonly IFileUploader _fileUploader;
    private readonly ISlideRepository _slideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SlideApplication(ISlideRepository slideRepository, IUnitOfWork unitOfWork)
    {
        _slideRepository = slideRepository;
        _unitOfWork = unitOfWork;
    }


    public OperationResult Create(CreateSlide command)
    {
        var operationResult = new OperationResult();
        _unitOfWork.BeginTran();
        try
        {
            var p1 = new Slide(command.Picture.ToString(),command.PictureAlt,command.PictureTitle,command.Heading,command.Title,command.BtnText,command.Link,command.BtnText);
            _slideRepository.Create(p1);
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditSlide command)
    {
        var operationResult = new OperationResult();
        _unitOfWork.BeginTran();
        try
        {
            var p1 = _slideRepository.Get(command.Id);
            p1.Edit(command.Picture.ToString(), command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.BtnText, command.Link, command.BtnText);
            //_productRepository.Edit(p1);
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
            _slideRepository.Get(id).Remove();
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
            _slideRepository.Get(id).Restore();
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
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