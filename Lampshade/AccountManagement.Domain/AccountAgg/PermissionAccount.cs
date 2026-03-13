using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Domain.RoleAgg;

public class PermissionAccount
{
    public PermissionAccount(int code)
    {
        Code = code;
    }

    public PermissionAccount(int code, string name)
    {
        Code = code;
        Name = name;
    }

    protected PermissionAccount()
    {
    }

    public long Id { get; private set; }
    public int Code { get; }
    public string Name { get; }

    public long AccountId { get; private set; }
    public Account Account { get; private set; }
}