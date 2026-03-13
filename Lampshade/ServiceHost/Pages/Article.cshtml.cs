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
    private readonly IArticleCategoryQuery _categoryQuery;
    private readonly ICommentApplication _commentApplication;
    public ArticleQueryModel Article;
    public List<ArticleCategoryQueryModel> ArticleCategories;
    public List<ArticleQueryModel> LatestArticles;

    public ArticleModel(IArticleQuery articleQuery, IArticleCategoryQuery categoryQuery,
        ICommentApplication commentApplication)
    {
        _articleQuery = articleQuery;
        _categoryQuery = categoryQuery;
        _commentApplication = commentApplication;
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
        var result = _commentApplication.Add(command);
        return RedirectToPage("/Article", new { Id = articleSlug });
    }
}