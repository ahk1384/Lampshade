namespace BlogManagement.Infrastructure.Configuration.Permissions;

public class BlogPermissions
{
    public const int BlogBase = 5000;
    public const int ArticleBase = BlogBase+100;
    public const int CreateArticle = ArticleBase+01;
    public const int EditArticle = ArticleBase+02;
    public const int SearchArticle = ArticleBase+03;
    public const int ArticleList = ArticleBase+04;
    
    
    public const int ArticleCategoryBase = BlogBase+200;
    public const int CreateArticleCategory = ArticleCategoryBase+01;
    public const int EditArticleCategory = ArticleCategoryBase+02;
    public const int SearchArticleCategory = ArticleCategoryBase+03;
    public const int ArticleCategoryList = ArticleBase+04;
}