using _0_Framework.Application;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application;

public class RoleApplication : IRoleApplication
{
    private readonly IRoleRepository _roleRepository;

    public RoleApplication(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public OperationResult Create(CreateRole command)
    {
        var operationResult = new OperationResult();
        if (_roleRepository.Exists(x => x.Name == command.Name))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _roleRepository.BeginTran();
        try
        {
            var role = new Role(command.Name, command.Description);
            _roleRepository.Create(role);
        }
        catch (Exception e)
        {
            _roleRepository.Rollback();
            return operationResult.Fail();
        }

        _roleRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditRole command)
    {
        var operationResult = new OperationResult();
        if (_roleRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _roleRepository.BeginTran();
        try
        {
            var role = _roleRepository.Get(command.Id);
            var permissions = SetPermissions(command.Permissions);
            role.Edit(command.Name, command.Description, permissions);
        }
        catch (Exception e)
        {
            _roleRepository.Rollback();
            return operationResult.Fail();
        }

        _roleRepository.CommitTran();
        return operationResult.Success();
    }

    public List<RoleViewModel> List()
    {
        return _roleRepository.List();
    }

    public EditRole GetDetails(long id)
    {
        return _roleRepository.GetDetails(id);
    }

    public List<Permission> SetPermissions(List<int> permissions)
    {
        var permissionAccounts = new List<Permission>();
        foreach (var permission in permissions)
        {
            if (!permissionAccounts.Any(x => x.Code == permission / 100 * 100))
                permissionAccounts.Add(new Permission(permission / 100 * 100));
            if (!permissionAccounts.Any(x => x.Code == permission / 1000 * 1000))
                permissionAccounts.Add(new Permission(permission / 1000 * 1000));
            permissionAccounts.Add(new Permission(permission));
        }

        return permissionAccounts;
    }
}