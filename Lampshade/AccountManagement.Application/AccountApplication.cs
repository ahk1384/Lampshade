using _0_Framework.Application;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application;

public class AccountApplication : IAccountApplication
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthHelper _authHelper;
    private readonly IFileUploader _fileUploader;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;

    public AccountApplication(IAccountRepository accountRepository, IFileUploader fileUploader, IAuthHelper authHelper,
        IPasswordHasher passwordHasher, IRoleRepository roleRepository)
    {
        _accountRepository = accountRepository;
        _fileUploader = fileUploader;
        _authHelper = authHelper;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
    }

    public AccountViewModel GetAccountBy(long id)
    {
        var account = _accountRepository.Get(id);
        return new AccountViewModel
        {
            Fullname = account.Fullname,
            Mobile = account.Mobile
        };
    }

    public OperationResult Register(CreateAccount command)
    {
        var operationResult = new OperationResult();
        if (_accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _accountRepository.BeginTran();
        try
        {
            var password = _passwordHasher.Hash(command.Password);
            var account = new Account(command.Fullname, command.Username, password, command.Mobile, command.RoleId,
                " ");
            _accountRepository.Create(account);
        }
        catch (Exception e)
        {
            _accountRepository.Rollback();
            return operationResult.Fail();
        }

        _accountRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditAccount command)
    {
        var operationResult = new OperationResult();
        if (_accountRepository.Exists(x =>
                (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _accountRepository.BeginTran();
        try
        {
            var Path = $"ProfilePhoto/{command.Username}";
            var pictureName = _fileUploader.Upload(command.ProfilePhoto, Path);
            var account = _accountRepository.Get(command.Id);
            var permissions = new List<PermissionAccount>();
            if (command.Permissions != null) permissions = SetPermissions(command.Permissions);
            account.Edit(command.Fullname, command.Username, command.Mobile, command.RoleId, pictureName, permissions);
        }
        catch (Exception e)
        {
            _accountRepository.Rollback();
            return operationResult.Fail();
        }

        _accountRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult ChangePassword(ChangePassword command)
    {
        var operationResult = new OperationResult();
        if (command.Password != command.RePassword)
            return operationResult.Fail(ApplicationMessages.PasswordsNotMatch);
        _accountRepository.BeginTran();
        try
        {
            var password = _passwordHasher.Hash(command.Password);
            var account = _accountRepository.Get(command.Id);
            account.ChangePassword(password);
        }
        catch (Exception e)
        {
            _accountRepository.Rollback();
            return operationResult.Fail();
        }

        _accountRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Login(Login command)
    {
        var operationResult = new OperationResult();
        try
        {
            var account = _accountRepository.GetBy(command.Username);
            if (account == null)
                return operationResult.Fail(ApplicationMessages.WrongUserPass);
            var res = _passwordHasher.Check(account.Password, command.Password);
            if (!res.Verified)
                return operationResult.Fail(ApplicationMessages.WrongUserPass);

            var rolepermissions = _roleRepository.Get(account.RoleId)
                .Permissions
                .Select(x => x.Code)
                .ToList();
            var accountPermissions = _accountRepository.Get(account.Id).Permissions.Select(x => x.Code).ToList();

            var permissions = rolepermissions.Union(accountPermissions).ToList();
            var authViewModel = new AuthViewModel(account.Id, account.RoleId, account.Role.Name, account.Fullname
                , account.Username, account.Mobile, permissions, account.ProfilePhoto);

            _authHelper.Signin(authViewModel);
            account.ChangePassword(command.Password);
        }
        catch (Exception e)
        {
            return operationResult.Fail(e.Message);
        }

        return operationResult.Success();
    }

    public EditAccount GetDetails(long id)
    {
        return _accountRepository.GetDetails(id);
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        return _accountRepository.Search(searchModel);
    }

    public void Logout()
    {
        _authHelper.SignOut();
    }

    public List<AccountViewModel> GetAccounts()
    {
        return _accountRepository.GetAccounts();
    }

    public List<PermissionAccount> SetPermissions(List<int> permissions)
    {
        var permissionAccounts = new List<PermissionAccount>();
        foreach (var permission in permissions)
        {
            if (!permissionAccounts.Any(x => x.Code == permission / 100 * 100))
                permissionAccounts.Add(new PermissionAccount(permission / 100 * 100));
            if (!permissionAccounts.Any(x => x.Code == permission / 1000 * 1000))
                permissionAccounts.Add(new PermissionAccount(permission / 1000 * 1000));

            permissionAccounts.Add(new PermissionAccount(permission));
        }

        return permissionAccounts;
    }
}