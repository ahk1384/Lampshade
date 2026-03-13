namespace CommentManagement.Infrastructure.Configuration.Permissions;

public class CommentPermissions
{
    public const int CommentBase = 4000;

    public const int ConfirmComment = CommentBase + 02;
    public const int CancelAndRestoreComment = CommentBase + 03;
    public const int SearchComment = CommentBase + 04;
    public const int CommentList = CommentBase + 05;
}