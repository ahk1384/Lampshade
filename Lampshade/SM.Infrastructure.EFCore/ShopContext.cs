using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore.Mapping;
using ShopManagementDomain.ProductAgg;
using ShopManagementDomain.ProductCategoryAgg;
using ShopManagementDomain.ProductPictureAgg;
using ShopManagementDomain.SlideAgg;
using SM.Infrastructure.EFCore.Mapping;
using SM.Infrastructure.EFCore.Migrations;

namespace SM.Infrastructure.EFCore;

public class ShopContext : DbContext
{
    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPicture> ProductPictures { get; set; }

    public DbSet<Slide> Slides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductCategoryMapping());
        modelBuilder.ApplyConfiguration(new ProductPictureMapping());
        modelBuilder.ApplyConfiguration(new ProductMapping());
        base.OnModelCreating(modelBuilder);
    }
}