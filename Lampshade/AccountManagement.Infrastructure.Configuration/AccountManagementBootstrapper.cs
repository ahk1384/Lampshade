using _0_Framework.Infrastructure;
using AccountManagement.Application;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Infrastructure.Configuration.Permissions;
using AccountManagement.Infrastructure.EFCore;
using AccountManagement.Infrastructure.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Infrastructure.Configuration;

public class AccountManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string? connectionString, string? databaseType)
    {
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IAccountApplication, AccountApplication>();

        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IRoleApplication, RoleApplication>();

        services.AddTransient<IPermissionExposer, AccountPermissionsExposer>();

        AccountPermissions.Configure();
        if (databaseType == "SqlServer")
        {
            services.AddDbContext<AccountContext>(x => x.UseSqlServer(connectionString));
        }
        else if (databaseType == "Mysql")
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));
            services.AddDbContext<AccountContext>(a => a.UseMySql(connectionString, serverVersion));
        }
    }
}