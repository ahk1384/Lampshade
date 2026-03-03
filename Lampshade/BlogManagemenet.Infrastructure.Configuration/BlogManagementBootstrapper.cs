using System.Security.Cryptography.X509Certificates;
using BlogManagement_Application.Contract.ArticleAgg;
using BlogManagement_Application.Contract.ArticleCategoryAgg;
using BlogManagement.Application;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.EFCore;
using BlogManagement.Infrastructure.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogManagement.Infrastructure.Configuration;

public class BlogManagementBootstrapper
{
    public static void Configure(IServiceCollection services , string? connectionString)
    {
        services.AddTransient<IArticleRepository, ArticleRepository>();
        services.AddTransient<IArticleApplication, ArticleApplication>();

        services.AddTransient<IArticleCategoryRepository, ArticleCategoryRepository>();
        services.AddTransient<IArticleCategoryApplication, ArticleCategoryApplication>();

        services.AddDbContext<BlogContext>(a=>a.UseSqlServer(connectionString));
    }
}