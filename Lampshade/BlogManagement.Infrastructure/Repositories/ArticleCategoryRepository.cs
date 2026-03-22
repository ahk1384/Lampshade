using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement_Application.Contract.ArticleCategoryAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repositories;

public class ArticleCategoryRepository : BaseRepository<long, ArticleCategory>, IArticleCategoryRepository
{
    private readonly BlogContext _blogContext;

    public ArticleCategoryRepository(BlogContext blogContext) : base(blogContext)
    {
        _blogContext = blogContext;
    }

    public string GetBySlug(long id)
    {
        return _blogContext.ArticleCategories.Select(x => new { x.Id, x.Slug }).FirstOrDefault(x => x.Id == id).Slug;
    }

    public EditArticleCategory GetDetails(long id)
    {
        return _blogContext.ArticleCategories.Select(x => new EditArticleCategory
        {
            Id = x.Id,
            Name = x.Name,
            CanonicalAddress = x.CanonicalAddress,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            ShowOrder = x.ShowOrder,
            Slug = x.Slug,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ArticleCategoryViewModel> GetArticleCategories()
    {
        return _blogContext.ArticleCategories.Select(x => new ArticleCategoryViewModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel, bool showDeleted)
    {
        var res = showDeleted
            ? _blogContext.ArticleCategories.Where(x => x.IsDeleted)
            : _blogContext.ArticleCategories.Where(x => !x.IsDeleted);
        var query = res
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryViewModel
            {
                Id = x.Id,
                Description = x.Description.Substring(0, Math.Min(x.Description.Length, 50)) + " ...",
                Name = x.Name,
                Picture = x.Picture,
                ShowOrder = x.ShowOrder,
                CreationDate = x.CreationDate.ToFarsi(),
                ArticlesCount = x.Articles.Count
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));

        return query.OrderByDescending(x => x.ShowOrder).ToList();
    }
}