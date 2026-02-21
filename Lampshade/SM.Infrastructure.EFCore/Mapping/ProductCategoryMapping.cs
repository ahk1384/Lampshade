using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementDomain.ProductCategoryAgg;

namespace SM.Infrastructure.EFCore.Mapping;

public class ProductCategoryMapping : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(255);
        builder.Property(x => x.Picture).HasMaxLength(150);
        builder.Property(x => x.PictureTitle).HasMaxLength(60);
        builder.Property(x => x.PictureAlt).HasMaxLength(155).IsRequired();
        builder.Property(x => x.Keywords).HasMaxLength(255).IsRequired();
        builder.Property(x => x.MetaDescription).HasMaxLength(255).IsRequired();
    }
}