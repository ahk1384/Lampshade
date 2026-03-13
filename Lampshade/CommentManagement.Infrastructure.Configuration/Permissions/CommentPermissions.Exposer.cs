using _0_Framework.Infrastructure;
using CommentManagement.Infrastructure.Configuration.Permissions;

namespace CommentManagement.Infrastructure.Configuration;

public class CommentPermissionsExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Comment", new List<PermissionDto>
                {
                    new (CommentPermissions.CommentList,"Comment List"),
                    new(CommentPermissions.SearchComment , "Search Comment"),
                    new(CommentPermissions.CancelAndRestoreComment ,  "CancelAndRestore Comment"),
                    new(CommentPermissions.ConfirmComment , "Confirm Comment"),
                }
            }
        };
    }
}