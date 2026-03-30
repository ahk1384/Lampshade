using _01_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers.User.BlogController;

[ApiController]
[Route("api/[controller]/")]
public class ArticleCategoryController : ControllerBase
{
    private readonly IArticleCategoryQuery _articleCategoryQuery;

    public ArticleCategoryController(IArticleCategoryQuery articleCategoryQuery)
    {
        _articleCategoryQuery = articleCategoryQuery;
    }

    [HttpGet]
    List<ArticleCategoryQueryModel> GetArticleCategories()
    {
        return _articleCategoryQuery.GetArticleCategories();
    }

    [HttpGet("{slug}")]
    public ArticleCategoryQueryModel GetArticleCategory(string slug)
    {
        return _articleCategoryQuery.GetArticleCategory(slug);
    }
}