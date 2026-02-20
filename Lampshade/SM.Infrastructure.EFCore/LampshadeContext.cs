using Microsoft.EntityFrameworkCore;
using ShopManagementDomain.ProductCategoryAgg;
using SM.Infrastructure.EFCore.Mapping;

namespace SM.Infrastructure.EFCore;

public class LampshadeContext:DbContext
{
    private readonly DbSet<ProductCategory> _products;
    public LampshadeContext(DbContextOptions<LampshadeContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductCategoryMapping());
        base.OnModelCreating(modelBuilder);
    }
}