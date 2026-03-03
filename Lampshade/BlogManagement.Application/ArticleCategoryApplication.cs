using _0_Framework.Application;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement_Application.Contract.ArticleCategoryAgg;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace BlogManagement.Application;

public class ArticleCategoryApplication : IArticleCategoryApplication
{
    private readonly IArticleCategoryRepository _articleCategoryRepository;
    private readonly IFileUploader _fileUploader;

    public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository, IFileUploader fileUploader)
    {
        _articleCategoryRepository = articleCategoryRepository;
        _fileUploader = fileUploader;
    }

    public OperationResult Create(CreateArticleCategory command)
    {
        var operationResult = new OperationResult();
        if (_articleCategoryRepository.Exists(x => x.Name == command.Name))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _articleCategoryRepository.BeginTran();
        try
        {
            var slug = command.Slug.Slugify();
            var pictureName = _fileUploader.Upload(command.Picture, slug);

            var p1 = new ArticleCategory(command.Name, pictureName, command.PictureAlt, command.PictureTitle,
                command.Description, command.ShowOrder, command.Slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress);
            _articleCategoryRepository.Create(p1);
        }
        catch (Exception e)
        {
            _articleCategoryRepository.Rollback();
            return operationResult.Fail();
        }

        _articleCategoryRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditArticleCategory command)
    {
        var operationResult = new OperationResult();

        if (_articleCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);

        _articleCategoryRepository.BeginTran();
        try
        {
            var slug = command.Slug.Slugify();
            var pictureName = _fileUploader.Upload(command.Picture, slug);

            var articleCategory = _articleCategoryRepository.Get(command.Id);
            if (articleCategory == null)
                return operationResult.Fail(ApplicationMessages.RecordNotFound);
            articleCategory.Edit(command.Name, pictureName, command.PictureAlt, command.PictureTitle,
                command.Description, command.ShowOrder, command.Slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress);
        }
        catch (Exception e)
        {
            _articleCategoryRepository.Rollback();
            return operationResult.Fail();
        }

        _articleCategoryRepository.CommitTran();
        return operationResult.Success();
    }

    public EditArticleCategory GetDetails(long id)
    {
        return _articleCategoryRepository.GetDetails(id);
    }

    public List<ArticleCategoryViewModel> GetArticleCategories()
    {
        return _articleCategoryRepository.GetArticleCategories();
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel, bool showDeleted)
    {
        return _articleCategoryRepository.Search(searchModel, showDeleted);
    }
}