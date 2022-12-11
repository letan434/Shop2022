using System;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopSolution.Data.Configurations
{
    public class ProductStartConfiguration : IEntityTypeConfiguration<ProductStart>
    {
        public void Configure(EntityTypeBuilder<ProductStart> builder)
        {
            builder.ToTable("ProductStarts");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.AppUser).WithMany(x => x.ProductStarts).HasForeignKey(x => x.UserId);
        }
    }
}
