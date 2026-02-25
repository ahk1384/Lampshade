using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace InventoryManagement.Infrastructure.EFCore.Mapping;

public class InventoryMapping : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventories");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId).IsRequired();
        builder.OwnsMany(x => x.Operations, modelbuilder =>
        {
            modelbuilder.ToTable("InventoryOperations");
            modelbuilder.HasKey(x => x.Id);
            modelbuilder.Property(x => x.Description).HasMaxLength(500);
            modelbuilder.Property(x => x.Operation);
            modelbuilder.Property(x => x.Count);
            modelbuilder.Property(x => x.OperatorId);
            modelbuilder.Property(x => x.OperationDate);
            modelbuilder.Property(x => x.CurrentCount);
            modelbuilder.Property(x => x.Description);
            modelbuilder.Property(x => x.OrderId);
            modelbuilder.Property(x => x.InventoryId);
            modelbuilder.WithOwner(x => x.Inventory).HasForeignKey(x => x.InventoryId);
        });

    }
}   