using System.Globalization;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagementDomain.ProductCategoryAgg;

namespace SM.Infrastructure.EFCore.Repositories;

public class ProductCategoryRepository : BaseRepository<long, ProductCategory>, IProductCategoryRepository
{
    private readonly ShopContext _context;

    public ProductCategoryRepository(ShopContext context, IProductCategoryRepository repository) : base(context)
    {
        _context = context;
    }


    public EditProductCategory? GetDetails(long id)
    {
        return _context.Products.Select(x => new EditProductCategory
        {
            Id = x.Id,
            Title = x.Title,
            CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture),
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Slug = x.Slug
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        var query = _context.Products.Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            Picture = x.Picture,
            CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture)
        });
        if (!string.IsNullOrWhiteSpace(searchModel.Name)) query = query.Where(x => x.Title.Contains(searchModel.Name));

        return query.OrderByDescending(x => x.Id).ToList();
    }
}