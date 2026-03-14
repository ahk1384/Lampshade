using _0_Framework.Application;
using _0_Framework.Infrastructure;

namespace AccountManagement.Infrastructure.Configuration.Permissions;

public class AccountPermissions : IPermissions
{
    public static void Configure()
    {
        PermissionsCodes.AddCode("account",AccountsBase);
        PermissionsCodes.AddCode("accountManagement",AccountBase);
        PermissionsCodes.AddCode("role",RoleBase);
    }
    public const int AccountsBase = 6000;
    public const int AccountBase = AccountsBase + 100;

    public const int CreateAccount = AccountBase + 01;
    public const int EditAccount = AccountBase + 02;
    public const int ChangeAccountPassword = AccountBase + 03;
    public const int SearchAccount = AccountBase + 04;
    public const int AccountList = AccountBase + 05;


    public const int RoleBase = AccountsBase + 200;

    public const int CreateRole = RoleBase + 01;
    public const int EditRole = RoleBase + 02;
    public const int RoleList = RoleBase + 03;
}


