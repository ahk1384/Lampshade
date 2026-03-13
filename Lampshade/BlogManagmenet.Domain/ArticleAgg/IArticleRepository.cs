using _0_Framework.Infrastructure;
using BlogManagement_Application.Contract.ArticleAgg;

namespace BlogManagement.Domain.ArticleAgg;

public interface IArticleRepository : IRepository<long, Article>
{
    EditArticle GetDetails(long id);
    Article GetWithCategory(long id);
    List<ArticleViewModel> Search(ArticleSearchModel searchModel, bool showDeleted);
}