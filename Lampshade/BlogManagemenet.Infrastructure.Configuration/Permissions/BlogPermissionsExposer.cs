using _0_Framework.Infrastructure;

namespace BlogManagement.Infrastructure.Configuration.Permissions;

public class BlogPermissionsExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Article", new List<PermissionDto>
                {
                    new(BlogPermissions.ArticleList, "Article List"),
                    new(BlogPermissions.CreateArticle, "Create Article"),
                    new(BlogPermissions.EditArticle, "Edit Article"),
                    new(BlogPermissions.SearchArticle, "Search Article")
                }
            },
            {
                "ArticleCategory", new List<PermissionDto>
                {
                    new(BlogPermissions.ArticleCategoryList, "ArticleCategory List"),
                    new(BlogPermissions.CreateArticleCategory, "Create ArticleCategory"),
                    new(BlogPermissions.EditArticleCategory, "Edit ArticleCategory"),
                    new(BlogPermissions.SearchArticleCategory, "Search ArticleCategory")
                }
            }
        };
    }
}