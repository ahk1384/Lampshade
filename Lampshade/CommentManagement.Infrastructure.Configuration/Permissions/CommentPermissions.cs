using _0_Framework.Application;
using _0_Framework.Infrastructure;

namespace CommentManagement.Infrastructure.Configuration.Permissions;

public class CommentPermissions : IPermissions
{
    public const int CommentBase = 4000;

    public const int ConfirmComment = CommentBase + 02;
    public const int CancelAndRestoreComment = CommentBase + 03;
    public const int SearchComment = CommentBase + 04;
    public const int CommentList = CommentBase + 05;

    public static void Configure()
    {
        PermissionsCodes.AddCode("comment", CommentBase);
    }
}