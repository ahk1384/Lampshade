using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.ProductAgg;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.SlideAgg;
using ShopManagement.Infrastructure.EFCore.Repository;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.ProductCategoryAgg;
using ShopManagementDomain.ProductPictureAgg;
using ShopManagementDomain.SlideAgg;
using SM.Infrastructure.EFCore;
using SM.Infrastructure.EFCore.Repositories;

namespace ShopManagement.Infrastructure.Configuration;

public class ShopManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string? connectionString)
    {
        services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();

        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IProductApplication, ProductApplication>();

        services.AddTransient<IProductPictureRepository, ProductPictureRepository>();
        services.AddTransient<IProductPictureApplication, ProductPictureApplication>();

        services.AddTransient<ISlideApplication, SlideApplication>();
        services.AddTransient<ISlideRepository, SlideRepository>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));

    }
}