using _0_Framework.Infrastructure;
using System.Linq.Expressions;
using DiscountManagement.Application.Contracts.ColleagueDiscount;

namespace DiscountManagement.Domain.ColleagueDiscountAgg;

public interface IColleagueDiscountRepository : IRepository<long, ColleagueDiscount>
{
    EditColleagueDiscount GetDetails(long id);

    List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel, bool watchDeleted);
}