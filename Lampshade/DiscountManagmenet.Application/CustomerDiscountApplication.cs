using _0_Framework.Application;
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
        if (_customerDiscountRepository.Exists(x => !x.IsDeleted &&
                                                    ((x.StartDate < command.EndDate.ToGeorgianDateTime() &&
                                                      command.EndDate.ToGeorgianDateTime() < x.EndDate) ||
                                                     (command.StartDate.ToGeorgianDateTime() > x.StartDate &&
                                                      command.StartDate.ToGeorgianDateTime() < x.EndDate)) &&
                                                    x.ProductId == command.ProductId))
            return operationResult.Fail(ApplicationMessages.CofilictInDate);

        if (command.StartDate.ToGeorgianDateTime() > command.EndDate.ToGeorgianDateTime())
            return operationResult.Fail(ApplicationMessages.InvalidDateRange);

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
                !x.IsDeleted && x.Id == command.Id && x.ProductId == command.ProductId &&
                x.DiscountRate == command.DiscountRate &&
                x.StartDate == command.StartDate.ToGeorgianDateTime() &&
                x.EndDate == command.EndDate.ToGeorgianDateTime()))

            return operationResult.Fail(ApplicationMessages.NotChanged);

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
            if (_customerDiscountRepository.Exists(x => !x.IsDeleted &&
                                                        (x.EndDate > discount.StartDate ||
                                                         x.StartDate < discount.EndDate) &&
                                                        x.ProductId == discount.ProductId))
                return operationResult.Fail(ApplicationMessages.CofilictInDate);
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

    private bool IsDateConfilicted(DateTime startDateA, DateTime endDateA, DateTime startDateB, DateTime endDateB)
    {
        if (startDateA < endDateB && endDateB < endDateA) return false;
        if (startDateB > startDateA && startDateB < endDateA) return false;
        return true;
    }
}