using Microsoft.EntityFrameworkCore;
using ShopManagementDomain.ProductCategoryAgg;
using SM.Infrastructure.EFCore.Mapping;

namespace SM.Infrastructure.EFCore;

public class ShopContext : DbContext
{
    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public DbSet<ProductCategory> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(ProductCategoryMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}