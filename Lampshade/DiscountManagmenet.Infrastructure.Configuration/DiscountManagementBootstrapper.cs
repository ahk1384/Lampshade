using _0_Framework.Infrastructure;
using DiscountManagemenet.Infrastructure.EFCore;
using DiscountManagmenet.Application;
using DiscountManagmenet.Application.Contracts.CustomerDiscount;
using DiscountManagmenet.Infrastructure.EFCore.Repositories;
using DiscountManagment.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountManagemenet.Infrastructure.Configuration;

public class DiscountManagementBootstrapper 
{
    public static void Configure(IServiceCollection services, string? connectionString)
    {
        services.AddTransient<ICustomerDiscountRepository, CustomerDiscountRepository>();
        services.AddTransient<ICustomerDiscountApplication, CustomerDiscountApplication>();

        services.AddTransient<IUnitOfWork, UnitOfWorkDiscount>();
        services.AddDbContext<DiscountContext>(x => x.UseSqlServer(connectionString));
    }
}