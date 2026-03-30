using _01_LampshadeQuery.Contracts.Article;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers.User.BlogController;

[ApiController]
[Route("api/Article/")]
public class ArticleController : ControllerBase
{
    private readonly IArticleQuery _articleQuery;

    public ArticleController(IArticleQuery articleQuery)
    {
        _articleQuery = articleQuery;
    }

    [HttpGet("Latest")]
    public List<ArticleQueryModel> LatestArticles()
    {
        return _articleQuery.LatestArticles();
    }

    [HttpGet("{slug}")]
    public ArticleQueryModel GetArticleDetails(string slug)
    {
        return _articleQuery.GetArticleDetails(slug);
    }
}