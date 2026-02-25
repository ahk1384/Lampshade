using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;

namespace DiscountManagement.Application;

public class CustomerDiscountApplication : ICustomerDiscountApplication
{
    private readonly ICustomerDiscountRepository _customerDiscountRepository;

    public CustomerDiscountApplication(ICustomerDiscountRepository customerDiscountRepository)
    {
        _customerDiscountRepository = customerDiscountRepository;
    }

    public OperationResult Define(DefineCustomerDiscount command)
    {
        var operationResult = new OperationResult();
        if (_customerDiscountRepository.Exists(x =>
                x.DiscountRate == command.DiscountRate && x.ProductId == command.ProductId))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _customerDiscountRepository.BeginTran();
        try
        {
            var discount = new CustomerDiscount(command.ProductId, command.DiscountRate,
                command.StartDate.ToGeorgianDateTime(), command.EndDate.ToGeorgianDateTime(), command.Reason);
            _customerDiscountRepository.Create(discount);
        }
        catch (Exception e)
        {
            _customerDiscountRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _customerDiscountRepository.CommitTran();
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

        _customerDiscountRepository.BeginTran();
        try
        {
            var discount = _customerDiscountRepository.Get(command.Id);
            discount.Edit(command.ProductId, command.DiscountRate,
                command.StartDate.ToGeorgianDateTime(), command.EndDate.ToGeorgianDateTime(), command.Reason);
        }
        catch (Exception e)
        {
            _customerDiscountRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _customerDiscountRepository.CommitTran();
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
        _customerDiscountRepository.BeginTran();
        try
        {
            var discount = _customerDiscountRepository.Get(id);
            discount.Remove();
        }
        catch (Exception e)
        {
            _customerDiscountRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _customerDiscountRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _customerDiscountRepository.BeginTran();
        try
        {
            var discount = _customerDiscountRepository.Get(id);
            discount.Restore();
        }
        catch (Exception e)
        {
            _customerDiscountRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _customerDiscountRepository.CommitTran();
        return operationResult.Success();
    }
}