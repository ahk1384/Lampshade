using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductAgg;
using ShopManagementDomain.ProductAgg;
using System.Linq.Expressions;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Http.Internal;
using ShopManagement.Application.Contracts.ProductCategoryAgg;
using ShopManagementDomain.ProductCategoryAgg;

namespace SM.Infrastructure.EFCore.Repositories;

public class ProductRepository : BaseRepository<long, Product>, IProductRepository
{
    private readonly ShopContext _context;
    private readonly IProductCategoryRepository _productCategoryRepository;
    public ProductRepository(ShopContext context, IProductCategoryRepository productCategoryRepository) : base(context)
    {
        _context = context;
        _productCategoryRepository = productCategoryRepository;
    }


    public EditProduct GetDetails(long id)
    {
        return _context.Products.Select(x => new EditProduct()
        {
            Id = x.Id,
            Name = x.Name,
            Code = x.Code,
            CategoryId = x.CategoryId,
            Slug = x.Slug,
            Picture = new FormFile(new MemoryStream(x.Picture.Length), 0, x.Picture.Length, "file", x.Picture),
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Description = x.Description,
            Categories = _productCategoryRepository.GetProductCategories(),
            ShortDescription = x.ShortDescription
        }).FirstOrDefault(x => x.Id== id);
    }

    public List<ProductViewModel> Search(ProductSearchModel searchModel, bool showDeleted)
    {
        var res = showDeleted
            ? _context.Products.Where(x => x.IsDeleted)
            : _context.Products.Where(x => !x.IsDeleted);
        var query = res.Include(x => x.Category).Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category.Title,
                CategoryId = x.CategoryId,
                Code = x.Code,
                Picture = x.Picture,
                CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture)
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));

        if (!string.IsNullOrWhiteSpace(searchModel.Code))
            query = query.Where(x => x.Code.Contains(searchModel.Code));

        if (searchModel.CategoryId > 0)
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);

        return query.OrderByDescending(x => x.Id).ToList();
    }

    public Product GetProductWithCategory(long id)
    {
        return _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
    }

    public List<ProductViewModel> GetProducts()
    {
        return _context.Products.Include(x => x.Category).Select(x => new ProductViewModel()
        {
            Id = x.Id,
            Name = x.Name,
            Category = x.Category.Title,
            CategoryId = x.CategoryId,
            Code = x.Code,
            Picture = x.Picture,
            CreationDate = x.CreationDate.ToString(CultureInfo.InvariantCulture)
        }).ToList();
    }

    public List<EditProduct> GetList()
    {
        return _context.Products.Select(x => new EditProduct()
        {
            Id = x.Id,
            Name = x.Name,
            Code = x.Code,
            CategoryId = x.CategoryId,
            Slug = x.Slug,
            Picture = new FormFile(new MemoryStream(x.Picture.Length), 0, x.Picture.Length, "file", x.Picture),
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Description = x.Description,
            ShortDescription = x.ShortDescription
        }).ToList();
    }
}