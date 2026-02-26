using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using DiscountManagement.Domain.CustomerDiscountAgg;

namespace DiscountManagement.Application;

public class ColleagueDiscountApplication : IColleagueDiscountApplication
{
    private readonly IColleagueDiscountRepository _repository;


    public ColleagueDiscountApplication(IColleagueDiscountRepository repository)
    {
        _repository = repository;
    }

    public OperationResult Define(DefineColleagueDiscount command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x =>
                x.DiscountRate == command.DiscountRate && x.ProductId == command.ProductId))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _repository.BeginTran();
        try
        {
            var discount = new ColleagueDiscount(command.ProductId, command.DiscountRate);
            _repository.Create(discount);
        }
        catch (Exception e)
        {
            _repository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _repository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditColleagueDiscount command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x =>
                x.DiscountRate == command.DiscountRate && x.ProductId == command.ProductId))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _repository.BeginTran();
        try
        {
            var discount = _repository.Get(command.Id);
            discount.Edit(command.ProductId, command.DiscountRate);
        }
        catch (Exception e)
        {
            _repository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _repository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Remove(long id)
    {
        var operationResult = new OperationResult();
        _repository.BeginTran();
        try
        {
            var discount = _repository.Get(id);
            discount.Remove();
        }
        catch (Exception e)
        {
            _repository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _repository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _repository.BeginTran();
        try
        {
            var discount = _repository.Get(id);
            discount.Restore();
        }
        catch (Exception e)
        {
            _repository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _repository.CommitTran();
        return operationResult.Success();
    }

    public EditColleagueDiscount GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel , bool watchDeleted)
    {
        return _repository.Search(searchModel, watchDeleted);
    }
}