using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagementDomain.ProductCategoryAgg;
using SM.Infrastructure.EFCore;
using SM.Infrastructure.EFCore.Repositories;

namespace ShopManagement.Infrastructure.Configuration;

public class ShopManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string? connectionString)
    {
        services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));

    }
}