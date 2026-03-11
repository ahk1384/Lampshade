using _01_LampshadeQuery.Contracts.Article;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponent;

public class LatestArticlesViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly IArticleQuery _articleQuery;

    public LatestArticlesViewComponent(IArticleQuery articleQuery)
    {
        _articleQuery = articleQuery;
    }

    public IViewComponentResult Invoke()
    {
        var articles = _articleQuery.LatestArticles();
        return View(articles);
    }
}