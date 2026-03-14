using _0_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;

namespace AccountManagement.Domain.AccountAgg;

public interface IAccountRepository : IRepository<long, Account>
{
    Account GetBy(string username);
    EditAccount GetDetails(long id);
    List<AccountViewModel> GetAccounts();
    List<AccountViewModel> Search(AccountSearchModel searchModel);
}