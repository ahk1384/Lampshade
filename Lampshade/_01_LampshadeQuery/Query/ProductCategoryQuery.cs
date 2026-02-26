using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.EntityFrameworkCore;
using ShopManagementDomain.ProductAgg;
using SM.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductCategoryQuery : IProductCategoryQuery
{
    private readonly ShopContext _context;

    public ProductCategoryQuery(ShopContext context)
    {
        _context = context;
    }

    public ProductCategoryQueryModel GetProductCategoryWithProducstsBy(string slug)
    {
        throw new NotImplementedException();
    }

    public List<ProductCategoryQueryModel> GetProductCategories()
    {
        return _context.ProductCategories.Include(x => x.Products).ThenInclude(x => x.Category).Select(x => new ProductCategoryQueryModel
        {
            Id = x.Id,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Name = x.Title,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Products = MapProducts(x.Products),
            Slug = x.Slug,
        }).ToList();
    }

    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        var query = _context.ProductCategories.Include(x => x.Products).ThenInclude(x => x.Category);
        return query.Select(x => new ProductCategoryQueryModel
        {
            Id = x.Id,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Name = x.Title,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Slug = x.Slug,
            Products = MapProducts(x.Products)
        }).ToList();
    }

    private static List<ProductQueryModel> MapProducts(List<Product> products)
    {
        var res = new List<ProductQueryModel>();
        products.ForEach(x => res.Add(new ProductQueryModel
        {
            Id = x.Id,
            Category = x.Category.Title,
            Name = x.Name,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ShortDescription = x.ShortDescription,
            Slug = x.Slug

        }));
        return res;
    }
}