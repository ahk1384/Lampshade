using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ArticleCategoryModel : PageModel
    {
        private readonly IArticleCategoryQuery _categoryQuery;
        private readonly IArticleQuery _articleQuery;

        public List<ArticleCategoryQueryModel> ArticleCategories;
        public ArticleCategoryQueryModel ArticleCategory;
        public List<ArticleQueryModel> LatestArticles;

        public ArticleCategoryModel(IArticleCategoryQuery categoryQuery, IArticleQuery articleQuery)
        {
            _categoryQuery = categoryQuery;
            _articleQuery = articleQuery;
        }

        public void OnGet(string id)
        {
            ArticleCategories = _categoryQuery.GetArticleCategories();
            LatestArticles = _articleQuery.LatestArticles();
            ArticleCategory = _categoryQuery.GetArticleCategory(id);
        }
    }
}
