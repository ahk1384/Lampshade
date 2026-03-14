using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repositories;

public class RoleRepository : BaseRepository<long, Role>, IRoleRepository
{
    private readonly AccountContext _context;

    public RoleRepository(AccountContext context, IAccountRepository accountRepository) : base(context)
    {
        _context = context;
    }

    public List<RoleViewModel> List()
    {
        return _context.Roles.Select(x => new RoleViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CreationDate = x.CreationDate.ToFarsi()
        }).ToList();
    }

    public EditRole GetDetails(long id)
    {
        var role = _context.Roles.Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                MappedPermissions = MapPermissions(x.Permissions)
            }).AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        role.Permissions = role.MappedPermissions.Select(x => x.Code).ToList();

        return role;
    }

    private static List<PermissionDto> MapPermissions(IEnumerable<Permission> permissions)
    {
        return permissions.Select(x => new PermissionDto(x.Code, x.Name)).ToList();
    }
}