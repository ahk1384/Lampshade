using _0_Framework.Infrastructure;

namespace AccountManagement.Infrastructure.Configuration.Permissions;

public class AccountPermissionsExposer : IPermissionExposer
{
    public Dictionary<String, List<PermissionDto>> Expose()
    {
        return new Dictionary<String, List<PermissionDto>>
        {
            {
                "Account", new List<PermissionDto>
                {
                    new(AccountPermissions.AccountList,"Account List"),
                    new(AccountPermissions.CreateAccount, "Create Account"),
                    new(AccountPermissions.EditAccount, "Edit Account"),
                    new(AccountPermissions.SearchAccount ,  "Search Account"),
                    new(AccountPermissions.ChangeAccountPassword, "Change Account Password")
                }
            },
            {   
                "Role", new List<PermissionDto>
                {
                    new (AccountPermissions.RoleList,"Role List"),
                    new(AccountPermissions.CreateRole , "Create Role"),
                    new(AccountPermissions.EditRole , "Edit Role")
                }
            },
        };
    }
}