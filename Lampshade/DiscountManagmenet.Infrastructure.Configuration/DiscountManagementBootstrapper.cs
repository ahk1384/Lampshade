using _0_Framework.Infrastructure;
using DiscountManagement.Application;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using DiscountManagement.Domain.CustomerDiscountAgg;
using DiscountManagement.Infrastructure.Configuration.Permissions;
using DiscountManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountManagement.Infrastructure.Configuration;

public class DiscountManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string? connectionString, string? databaseType)
    {
        services.AddTransient<ICustomerDiscountRepository, CustomerDiscountRepository>();
        services.AddTransient<ICustomerDiscountApplication, CustomerDiscountApplication>();

        services.AddTransient<IColleagueDiscountRepository, ColleagueDiscountRepository>();
        services.AddTransient<IColleagueDiscountApplication, ColleagueDiscountApplication>();

        services.AddTransient<IPermissionExposer, DiscountPermissionsExposer>();

        DiscountPermissions.Configure();
        if (databaseType == "SqlServer")
        {
            services.AddDbContext<DiscountContext>(x => x.UseSqlServer(connectionString));
        }
        else if (databaseType == "Mysql")
        {
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<DiscountContext>(x => x.UseMySql(connectionString, serverVersion));
        }
    }
}