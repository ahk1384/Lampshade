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

    
    public List<ProductCategoryViewModel> GetProductCategories()
    {
        return _repository.GetProductCategories();
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
            _repository.Get(id).Remove();
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
            _repository.Get(id).Restore();
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

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel,bool showDeleted =false)
    {
        return _repository.Search(searchModel,showDeleted);
    }
}