using _0_Framework.Infrastructure;
using CommentManagement.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;
using CommentManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommentManagement.Infrastructure.Configuration;

public class CommentManagementBootstrapper
{
    public static void Configure(IServiceCollection services, string? connectionString)
    {
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<ICommentApplication, CommentApplication>();

        services.AddTransient<IPermissionExposer, CommentPermissionsExposer>();
        services.AddDbContext<CommentContext>(x => x.UseSqlServer(connectionString));
    }
}