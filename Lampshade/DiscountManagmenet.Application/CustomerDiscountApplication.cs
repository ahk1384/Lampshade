using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagmenet.Application.Contracts.CustomerDiscount;
using DiscountManagment.Domain.CustomerDiscountAgg;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DiscountManagmenet.Application;

public class CustomerDiscountApplication : ICustomerDiscountApplication
{
    private readonly ICustomerDiscountRepository _customerDiscountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository, IUnitOfWork unitOfWork)
    {
        _customerDiscountRepository = customerDiscountRepository;
        _unitOfWork = unitOfWork;
    }

    public OperationResult Define(DefineCustomerDiscount command)
    {
        var operationResult = new OperationResult();
        if (_customerDiscountRepository.Exists(x =>
                x.DiscountRate == command.DiscountRate && x.ProductId == command.ProductId))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _unitOfWork.BeginTran();
        try
        {
            var discount = new CustomerDiscount(command.ProductId, command.DiscountRate,
                command.StartDate.ToGeorgianDateTime(), command.EndDate.ToGeorgianDateTime(), command.Reason);
            _customerDiscountRepository.Create(discount);
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail(e.Message);
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditCustomerDiscount command)
    {
        var operationResult = new OperationResult();
        if (_customerDiscountRepository.Exists(x =>
                x.DiscountRate == command.DiscountRate && x.ProductId == command.ProductId))

            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);

        if (command.StartDate.ToGeorgianDateTime() > command.EndDate.ToGeorgianDateTime())

            return operationResult.Fail(ApplicationMessages.InvalidDateRange);

        _unitOfWork.BeginTran();
        try
        {
            var discount = _customerDiscountRepository.Get(command.Id);
            discount.Edit(command.ProductId, command.DiscountRate,
                command.StartDate.ToGeorgianDateTime(), command.EndDate.ToGeorgianDateTime(), command.Reason);
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail(e.Message);
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }

    public EditCustomerDiscount GetDetails(long id)
    {
        return _customerDiscountRepository.GetDetails(id);
    }

    public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel, bool watchDeleted)
    {
        return _customerDiscountRepository.Search(searchModel, watchDeleted);
    }

    public OperationResult Remove(long id)
    {
        var operationResult = new OperationResult();
        _unitOfWork.BeginTran();
        try
        {
            var discount = _customerDiscountRepository.Get(id);
            discount.Remove();
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail(e.Message);
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
            var discount = _customerDiscountRepository.Get(id);
            discount.Restore();
        }
        catch (Exception e)
        {
            _unitOfWork.Rollback();
            return operationResult.Fail(e.Message);
        }

        _unitOfWork.CommitTran();
        return operationResult.Success();
    }
}