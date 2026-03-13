using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagementDomain.ProductPictureAgg;

namespace SM.Infrastructure.EFCore.Repositories;

public class ProductPictureRepository : BaseRepository<long, ProductPicture>, IProductPictureRepository
{
    private readonly ShopContext _context;

    public ProductPictureRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public EditProductPicture GetDetails(long id)
    {
        return _context.ProductPictures.Select(x => new EditProductPicture
        {
            Id = x.Id,
            ProductId = x.ProductId,
            // Assuming Picture is a byte array, you might want to handle it differently based on your requirements
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle
        }).FirstOrDefault(x => x.Id == id);
    }

    public ProductPicture GetWithProductAndCategory(long id)
    {
        return _context.ProductPictures.Include(x => x.Product).ThenInclude(x => x.Category)
            .FirstOrDefault(x => x.Id == id);
    }

    public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel, bool showDeleted = false)
    {
        var res = showDeleted
            ? _context.ProductPictures.Where(x => x.IsDeleted)
            : _context.ProductPictures.Where(x => !x.IsDeleted);

        if (searchModel.ProductPictureId.HasValue && searchModel.ProductPictureId.Value > 0)
            res = res.Where(x => x.Id == searchModel.ProductPictureId.Value);
        if (!string.IsNullOrWhiteSpace(searchModel.ProductName))
            res = res.Where(x => x.Product.Name.Contains(searchModel.ProductName));
        return res.Select(x => new ProductPictureViewModel
        {
            Id = x.Id,
            ProductId = x.ProductId,
            CreationDate = x.CreationDate.ToFarsi(),
            IsRemoved = x.IsDeleted,
            Picture = x.Picture,
            Product = x.Product.Name
        }).ToList();
    }
}