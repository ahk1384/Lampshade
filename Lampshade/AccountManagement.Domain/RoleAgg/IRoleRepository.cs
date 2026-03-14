using _0_Framework.Infrastructure;
using AccountManagement.Application.Contract.Role;

namespace AccountManagement.Domain.RoleAgg;

public interface IRoleRepository : IRepository<long, Role>
{
    List<RoleViewModel> List();
    EditRole GetDetails(long id);
}