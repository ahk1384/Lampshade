using System.Linq.Expressions;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagementDomain.ProductCategoryAgg;

namespace SM.Infrastructure.EFCore.Repositories;

public class ProductCategoryRepository : BaseRepository<long,ProductCategory>, IProductCategoryRepository
{
    private readonly LampshadeContext _context;
    public ProductCategoryRepository(LampshadeContext context, IProductCategoryRepository repository) : base(context)
    {
        _context = context;
    }
}