using _0_Framework.Infrastructure;
using DiscountManagmenet.Application.Contracts.CustomerDiscount;

namespace DiscountManagment.Domain.CustomerDiscountAgg;

public interface ICustomerDiscountRepository : IRepository<long,CustomerDiscount>
{
    EditCustomerDiscount GetDetails(long id);
    List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel, bool watchDeleted);
}