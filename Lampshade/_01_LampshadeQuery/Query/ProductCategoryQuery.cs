using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
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
        return _context.ProductCategories.Select(x => new ProductCategoryQueryModel()
        {
            Id = x.Id,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Name = x.Title,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Products = new List<ProductQueryModel>(),
            Slug = x.Slug,
        }).ToList();
    }

    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        throw new NotImplementedException();
    }
}