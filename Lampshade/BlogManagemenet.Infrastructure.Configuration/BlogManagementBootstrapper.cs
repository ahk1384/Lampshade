using _0_Framework.Infrastructure;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using _01_LampshadeQuery.Query;
using BlogManagement_Application.Contract.ArticleAgg;
using BlogManagement_Application.Contract.ArticleCategoryAgg;
using BlogManagement.Application;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.Configuration.Permissions;
using BlogManagement.Infrastructure.EFCore;
using BlogManagement.Infrastructure.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogManagement.Infrastructure.Configuration;

public class BlogManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string? connectionString, string? databaseType)
    {
        services.AddTransient<IArticleRepository, ArticleRepository>();
        services.AddTransient<IArticleApplication, ArticleApplication>();

        services.AddTransient<IArticleCategoryRepository, ArticleCategoryRepository>();
        services.AddTransient<IArticleCategoryApplication, ArticleCategoryApplication>();


        services.AddTransient<IArticleQuery, ArticleQuery>();
        services.AddTransient<IArticleCategoryQuery, ArticleCategoryQuery>();

        services.AddTransient<IPermissionExposer, BlogPermissionsExposer>();

        BlogPermissions.Configure();
        if (databaseType == "SqlServer")
        {
            services.AddDbContext<BlogContext>(x => x.UseSqlServer(connectionString));
        }
        else if (databaseType == "Mysql")
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));
            services.AddDbContext<BlogContext>(a => a.UseMySql(connectionString, serverVersion));
        }
    }
}