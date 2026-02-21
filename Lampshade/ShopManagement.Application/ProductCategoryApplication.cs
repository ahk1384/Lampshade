using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagementDomain.ProductCategoryAgg;
using System.Globalization;

namespace ShopManagement.Application;

public class ProductCategoryApplication(IProductCategoryRepository repository, IUnitOfWork unitOfWork)
    : IProductCategoryApplication
{
    private readonly IProductCategoryRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public ProductCategoryViewModel Get(long id)
    {
        var p1 = _repository.Get(id);
        return new ProductCategoryViewModel
        {
            Title = p1.Title,
            CreationDate = p1.CreationDate.ToString(CultureInfo.InvariantCulture),
            Description = p1.Description,
            Id = p1.Id,
            Picture = p1.Picture
        };
    }

    public List<ProductCategoryViewModel> GetAll()
    {
        return _repository.GetAll().Select(x => new ProductCategoryViewModel
        {
            Title = x.Title,
            CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture),
            Description = x.Description,
            Id = x.Id,
            Picture = x.Picture
        }).ToList();
    }

    public OperationResult Add(CreateProductCategory productCategory)
    {
        var operationResult = new OperationResult();
        _unitOfWork.BeginTran();
        try
        {
            var p1 = new ProductCategory(productCategory.Title, productCategory.Picture, productCategory.PictureAlt,
                productCategory.PictureTitle, productCategory.MetaDescription, productCategory.Keywords,
                productCategory.Slug);
            _repository.Create(p1);
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditProductCategory productCategory)
    {
        var operationResult = new OperationResult();
        _unitOfWork.BeginTran();
        try
        {
            var p1 = _repository.Get(productCategory.Id);
            p1.Edit(productCategory.Title, productCategory.Picture, productCategory.PictureAlt,
                productCategory.PictureTitle, productCategory.MetaDescription, productCategory.Keywords,
                productCategory.Slug);
            //_repository.Edit(p1);
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
            _repository.Remove(id);
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
            _repository.Restore(id);
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail();
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    List<EditProductCategory> IProductCategoryApplication.GetList()
    {
        return _repository.GetList();
    }

    public EditProductCategory GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        return _repository.Search(searchModel);
    }
}