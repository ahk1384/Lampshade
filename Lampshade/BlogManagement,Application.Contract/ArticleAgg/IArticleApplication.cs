using _0_Framework.Application;

namespace BlogManagement_Application.Contract.ArticleAgg;

public interface IArticleApplication
{
    OperationResult Create(CreateArticle command);
    OperationResult Edit(EditArticle command);
    EditArticle GetDetails(long id);
    List<ArticleViewModel> Search(ArticleSearchModel searchModel, bool showDeleted);
}