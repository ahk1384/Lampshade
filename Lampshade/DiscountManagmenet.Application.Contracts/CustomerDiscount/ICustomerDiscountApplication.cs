using _0_Framework.Application;

namespace DiscountManagmenet.Application.Contracts.CustomerDiscount;

public interface ICustomerDiscountApplication
{
    OperationResult Define(DefineCustomerDiscount command);
    OperationResult Edit(EditCustomerDiscount command);
    EditCustomerDiscount GetDetails(long id);
    List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel , bool watchDeleted);
    OperationResult Remove(long id);
    OperationResult Restore(long id);
}