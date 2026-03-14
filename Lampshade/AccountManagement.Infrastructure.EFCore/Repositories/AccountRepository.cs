using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repositories;

public class AccountRepository : BaseRepository<long, Account>, IAccountRepository
{
    private readonly AccountContext _context;

    public AccountRepository(AccountContext context) : base(context)
    {
        _context = context;
    }

    public Account GetBy(string username)
    {
        return _context.Accounts.FirstOrDefault(x => x.Username == username);
    }

    public EditAccount GetDetails(long id)
    {
        var account = _context.Accounts.Select(x => new EditAccount
        {
            Id = x.Id,
            Username = x.Username,
            Fullname = x.Fullname,
            Mobile = x.Mobile,
            RoleId = x.RoleId,
            MappedPermissions = MapPermissions(x.Permissions)
        }).AsNoTracking().FirstOrDefault(x => x.Id == id);

        account.Permissions = account.MappedPermissions.Select(x => x.Code).ToList();

        return account;
    }

    public List<AccountViewModel> GetAccounts()
    {
        return _context.Accounts.Select(x => new AccountViewModel
        {
            Fullname = x.Fullname,
            Id = x.Id
        }).ToList();
    }

    public List<AccountViewModel> Search(AccountSearchModel searchModel)
    {
        var query = _context.Accounts.Include(x => x.Role).Select(x => new AccountViewModel
        {
            Id = x.Id,
            Fullname = x.Fullname,
            Mobile = x.Mobile,
            ProfilePhoto = x.ProfilePhoto,
            Role = x.Role.Name,
            RoleId = x.RoleId,
            Username = x.Username,
            CreationDate = x.CreationDate.ToFarsi()
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Fullname))
            query = query.Where(x => x.Fullname.Contains(searchModel.Fullname));

        if (!string.IsNullOrWhiteSpace(searchModel.Username))
            query = query.Where(x => x.Username.Contains(searchModel.Username));

        if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
            query = query.Where(x => x.Mobile.Contains(searchModel.Mobile));

        if (searchModel.RoleId > 0)
            query = query.Where(x => x.RoleId == searchModel.RoleId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
    
    private static List<PermissionDto> MapPermissions(IEnumerable<PermissionAccount> permissions)
    {
        return permissions.Select(x => new PermissionDto(x.Code, x.Name)).ToList();
    }
}