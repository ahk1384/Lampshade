using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagementDomain.CartAgg;

namespace SM.Infrastructure.EFCore.Mapping;

public class CartMapping :  IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AccountId);
        builder.Property(x => x.TotalAmount);
        builder.Property(x => x.DiscountAmount);
        builder.Property(x => x.PayAmount);
        builder.Property(x => x.PaymentMethod);
        builder.OwnsMany(x => x.Items, navigationBuilder =>
        {
            navigationBuilder.ToTable("CartItems");
            navigationBuilder.HasKey(x => x.Id);
            navigationBuilder.Property(x => x.ProductId);
            navigationBuilder.Property(x => x.Count);
            navigationBuilder.Property(x => x.UnitPrice);
            navigationBuilder.Property(x => x.DiscountRate);
            navigationBuilder.Property(x => x.CartId);
            navigationBuilder.WithOwner(x => x.Cart).HasForeignKey(x => x.CartId);
        });
    }
    
}