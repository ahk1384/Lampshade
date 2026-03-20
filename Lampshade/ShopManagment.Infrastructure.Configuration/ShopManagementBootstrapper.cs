using _0_Framework.Infrastructure;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
using _01_LampshadeQuery.Contracts.Slide;
using _01_LampshadeQuery.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Cart;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Application.Contracts.ProductAgg;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.SlideAgg;
using ShopManagement.Domain.Services;
using ShopManagement.Infrastructure.AccountAcl;
using ShopManagement.Infrastructure.Configuration.Permissions;
using ShopManagement.Infrastructure.EFCore.Repository;
using ShopManagement.Infrastructure.InventoryAcl;
using ShopManagementDomain.OrderAgg;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.ProductCategoryAgg;
using ShopManagementDomain.ProductPictureAgg;
using ShopManagementDomain.Services;
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

        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderApplication, OrderApplication>();

        services.AddTransient<IShopInventoryAcl, ShopInventoryAcl>();
        services.AddTransient<IShopAccountAcl, ShopAccountAcl>();

        services.AddTransient<ISlideQuery, SlideQuery>();
        services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();
        services.AddTransient<IProductQuery, ProductQuery>();
        services.AddTransient<ICartQuery, CartQuery>();

        services.AddTransient<IPermissionExposer, ShopPermissionsExposer>();
        services.AddTransient<ICartCalculatorService, CartCalculatorService>();

        services.AddSingleton<ICartService, CartService>();

        ShopPermissions.Configure();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));
        services.AddDbContext<ShopContext>(x => x.UseMySql(connectionString, serverVersion));
    }
}