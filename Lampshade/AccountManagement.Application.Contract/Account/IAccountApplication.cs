using _0_Framework.Application;

namespace AccountManagement.Application.Contract.Account;

public interface IAccountApplication
{
    AccountViewModel GetAccountBy(long id);
    OperationResult Register(CreateAccount command);
    OperationResult Edit(EditAccount command);
    OperationResult ChangePassword(ChangePassword command);
    Task<OperationResult> Login(Login command);
    EditAccount GetDetails(long id);
    List<AccountViewModel> Search(AccountSearchModel searchModel);
    void Logout();
    List<AccountViewModel> GetAccounts();
}