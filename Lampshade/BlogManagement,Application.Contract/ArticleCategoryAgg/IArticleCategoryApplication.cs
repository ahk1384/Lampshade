using _0_Framework.Application;

namespace BlogManagement_Application.Contract.ArticleCategoryAgg;

public interface IArticleCategoryApplication
{
    OperationResult Create(CreateArticleCategory command);

    OperationResult Edit(EditArticleCategory command);

    EditArticleCategory GetDetails(long id);

    List<ArticleCategoryViewModel> GetArticleCategories();
    List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel, bool showDeleted);
}