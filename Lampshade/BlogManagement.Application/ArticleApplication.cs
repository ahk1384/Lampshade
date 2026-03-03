using _0_Framework.Application;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement_Application.Contract.ArticleAgg;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.CodeAnalysis;

namespace BlogManagement.Application;

public class ArticleApplication : IArticleApplication
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleCategoryRepository _articleCategoryRepository;
    private readonly IFileUploader _fileUploader;
    public ArticleApplication(IArticleRepository articleRepository, IFileUploader fileUploader, IArticleCategoryRepository articleCategoryRepository)
    {
        _articleRepository = articleRepository;
        _fileUploader = fileUploader;
        _articleCategoryRepository = articleCategoryRepository;
    }

    public OperationResult Create(CreateArticle command)
    {
        var operationResult = new OperationResult();
        if (_articleRepository.Exists(x => x.Title == command.Title))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);
        _articleRepository.BeginTran();
        try
        {
            var slug = command.Slug.Slugify();
            var categorySlug = _articleCategoryRepository.GetBySlug(command.CategoryId);
            var picturePath = $"{categorySlug}/{command.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture, picturePath);
            var publishDate = command.PublishDate.ToGeorgianDateTime();

            var p1 = new Article(command.Title,command.ShortDescription,command.Description,pictureName,command.PictureAlt,command.PictureTitle,publishDate,command.Slug,command.Keywords,command.MetaDescription,command.CanonicalAddress,command.CategoryId);
            _articleRepository.Create(p1);
        }
        catch (Exception e)
        {
            _articleRepository.Rollback();
            return operationResult.Fail();
        }

        _articleRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Edit(EditArticle command)
    {
        var operationResult = new OperationResult();

        if (_articleRepository.Exists(x => x.Title == command.Title && x.Id != command.Id))
            return operationResult.Fail(ApplicationMessages.DuplicatedRecord);

        _articleRepository.BeginTran();
        try
        {
            var slug = command.Slug.Slugify();
            var categorySlug = _articleCategoryRepository.GetBySlug(command.CategoryId);
            var picturePath = $"{categorySlug}/{command.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture, picturePath);
            var publishDate = command.PublishDate.ToGeorgianDateTime();

            var article = _articleRepository.GetWithCategory(command.Id);
            if (article == null)
                return operationResult.Fail(ApplicationMessages.RecordNotFound);
            article.Edit(command.Title, command.ShortDescription, command.Description, pictureName, command.PictureAlt,
                command.PictureTitle, publishDate, command.Slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress, command.CategoryId);
        }
        catch (Exception e)
        {
            _articleRepository.Rollback();
            return operationResult.Fail();
        }

        _articleRepository.CommitTran();
        return operationResult.Success();
    }

    public EditArticle GetDetails(long id)
    {
        return _articleRepository.GetDetails(id);
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel, bool showDeleted)
    {
        return _articleRepository.Search(searchModel, showDeleted);
    }
}