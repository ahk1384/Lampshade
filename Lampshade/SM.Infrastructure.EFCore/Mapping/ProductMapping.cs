using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementDomain.ProductAgg;

namespace SM.Infrastructure.EFCore.Mapping;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(15).IsRequired();
        builder.Property(x => x.ShortDescription).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Picture);
        builder.Property(x => x.PictureTitle).HasMaxLength(60);
        builder.Property(x => x.PictureAlt).HasMaxLength(155).IsRequired();
        builder.Property(x => x.Keywords).HasMaxLength(255).IsRequired();
        builder.Property(x => x.MetaDescription).HasMaxLength(255).IsRequired();

        builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
        builder.HasMany(x => x.ProductPictures).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
    }
}