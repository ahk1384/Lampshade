using System.Globalization;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagementDomain.ProductCategoryAgg;

namespace SM.Infrastructure.EFCore.Repositories;

public class ProductCategoryRepository : BaseRepository<long, ProductCategory>, IProductCategoryRepository
{
    private readonly ShopContext _context;

    public ProductCategoryRepository(ShopContext context) : base(context)
    {
        _context = context;
    }


    public EditProductCategory? GetDetails(long id)
    {
        return _context.ProductCategories.Select(x => new EditProductCategory
        {
            Id = x.Id,
            Title = x.Title,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Slug = x.Slug
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ProductCategoryViewModel> GetProductCategories()
    {
        return _context.ProductCategories.Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Title = x.Title
        }).ToList();
    }

    public ProductCategoryViewModel GetProductCategory(long id)
    {
        return _context.ProductCategories.Where(x => x.Id == id).Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Title = x.Title,
            CreationDate = x.CreationDate.ToFarsi(),
            Description = x.Description,
            Picture = x.Picture
        }).FirstOrDefault();
    }

    public List<EditProductCategory> GetList()
    {
        return _context.ProductCategories.Select(x => new EditProductCategory
        {
            Id = x.Id,
            Title = x.Title,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Slug = x.Slug
        }).ToList();
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel, bool showDeleted)
    {
        var res = showDeleted
            ? _context.ProductCategories.Where(x => x.IsDeleted)
            : _context.ProductCategories.Where(x => !x.IsDeleted);
       var query = res.Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            Picture = x.Picture,
            CreationDate = x.CreationDate.ToFarsi()
        });
        if (!string.IsNullOrWhiteSpace(searchModel.Name)) query = query.Where(x => x.Title.Contains(searchModel.Name));

        return query.OrderByDescending(x => x.Id).ToList();
    }
}