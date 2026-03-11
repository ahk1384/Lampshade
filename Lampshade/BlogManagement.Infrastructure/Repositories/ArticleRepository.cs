using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement_Application.Contract.ArticleAgg;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;


namespace BlogManagement.Infrastructure.EFCore.Repositories;

public class ArticleRepository : BaseRepository<long,Article>, IArticleRepository
{
    private readonly BlogContext _blogContext;
    public ArticleRepository(BlogContext blogContext) : base(blogContext)
    {
        _blogContext = blogContext;
    }

    public EditArticle GetDetails(long id)
    {
        return _blogContext.Articles.Select(x => new EditArticle
        {
            Id = x.Id,
            CanonicalAddress = x.CanonicalAddress,
            CategoryId = x.CategoryId,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            PublishDate = x.PublishDate.ToFarsi(),
            ShortDescription = x.ShortDescription,
            Slug = x.Slug,
            Title = x.Title
        }).FirstOrDefault(x => x.Id == id);
    }

    public Article GetWithCategory(long id)
    {
        return _blogContext.Articles.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel, bool showDeleted)
    {
        var res = showDeleted
            ? _blogContext.Articles.Where(x => x.IsDeleted)
            : _blogContext.Articles.Where(x => !x.IsDeleted);

        var query = res.Select(x => new ArticleViewModel
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            Category = x.Category.Name,
            Picture = x.Picture,
            PublishDate = x.PublishDate.ToFarsi(),
            ShortDescription = x.ShortDescription.Substring(0, Math.Min(x.ShortDescription.Length, 50)) + " ...",
            Title = x.Title
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Title))
            query = query.Where(x => x.Title.Contains(searchModel.Title));

        if (searchModel.CategoryId > 0)
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
}