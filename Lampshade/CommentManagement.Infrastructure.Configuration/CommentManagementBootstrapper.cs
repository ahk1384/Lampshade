using _0_Framework.Infrastructure;
using CommentManagement.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;
using CommentManagement.Infrastructure.Configuration.Permissions;
using CommentManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommentManagement.Infrastructure.Configuration;

public class CommentManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string? connectionString, string? databaseType)
    {
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<ICommentApplication, CommentApplication>();

        services.AddTransient<IPermissionExposer, CommentPermissionsExposer>();

        CommentPermissions.Configure();
        if (databaseType == "SqlServer")
        {
            services.AddDbContext<CommentContext>(x => x.UseSqlServer(connectionString));
        }
        else if (databaseType == "Mysql")
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));
            services.AddDbContext<CommentContext>(x => x.UseMySql(connectionString, serverVersion));
        }
    }
}