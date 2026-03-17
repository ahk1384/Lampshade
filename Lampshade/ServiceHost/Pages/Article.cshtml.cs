using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using CommentManagement.Application.Contracts.Comment;
using CommnetManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class ArticleModel : PageModel
{
    private readonly IArticleQuery _articleQuery;
    private readonly IAuthHelper _authHelper;
    private readonly IArticleCategoryQuery _categoryQuery;
    private readonly ICommentApplication _commentApplication;
    public ArticleQueryModel Article;
    public List<ArticleCategoryQueryModel> ArticleCategories;
    public List<ArticleQueryModel> LatestArticles;

    public ArticleModel(IArticleQuery articleQuery, IArticleCategoryQuery categoryQuery,
        ICommentApplication commentApplication, IAuthHelper authHelper)
    {
        _articleQuery = articleQuery;
        _categoryQuery = categoryQuery;
        _commentApplication = commentApplication;
        _authHelper = authHelper;
    }

    public void OnGet(string id)
    {
        Article = _articleQuery.GetArticleDetails(id);
        LatestArticles = _articleQuery.LatestArticles();
        ArticleCategories = _categoryQuery.GetArticleCategories();
    }

    public RedirectToPageResult OnPost(AddComment command, string articleSlug)
    {
        command.Type = CommentType.Article;
        if (_authHelper.IsAuthenticated()) command.Name = _authHelper.CurrentAccountInfo().Username;
        var result = _commentApplication.Add(command);
        return RedirectToPage("/Article", new { Id = articleSlug });
    }
}