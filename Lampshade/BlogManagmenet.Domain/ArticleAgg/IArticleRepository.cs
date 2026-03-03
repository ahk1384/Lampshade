using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement_Application.Contract.ArticleAgg;
using BlogManagement_Application.Contract.ArticleCategoryAgg;

namespace BlogManagement.Domain.ArticleAgg;

public interface IArticleRepository : IRepository<long, Article>
{
    EditArticle GetDetails(long id);
    Article GetWithCategory(long id);
    List<ArticleViewModel> Search(ArticleSearchModel searchModel,bool showDeleted);
}