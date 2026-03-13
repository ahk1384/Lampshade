using _0_Framework.Infrastructure;
using BlogManagement_Application.Contract.ArticleCategoryAgg;

namespace BlogManagement.Domain.ArticleCategoryAgg;

public interface IArticleCategoryRepository : IRepository<long, ArticleCategory>
{
    string GetBySlug(long id);
    EditArticleCategory GetDetails(long id);

    List<ArticleCategoryViewModel> GetArticleCategories();

    List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel, bool showDeleted);
}