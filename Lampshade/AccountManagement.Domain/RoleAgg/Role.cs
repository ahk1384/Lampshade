using _0_Framework.Domain;
using AccountManagement.Domain.AccountAgg;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AccountManagement.Domain.RoleAgg;

public class Role : EntityBase<long>
{
    protected Role()
    {

    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<Account> Accounts { get;}
    public List<Permission> Permissions { get; private set; }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
        Accounts = new List<Account>();
        Permissions = new List<Permission>();
    }

    public void Edit(string name, string description, List<Permission> permissions)
    {
        Name = name;
        Description = description;
        Permissions = permissions;
    }
}