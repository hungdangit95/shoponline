using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.EF.Extensions;

namespace ShopOnlineApp.Data.EF.Configurations
{
    public class FunctionConfiguration : DbEntityConfiguration<Function>
    {
        public override void Configure(EntityTypeBuilder<Function> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).IsRequired()
            .HasColumnType("varchar(128)");
            // etc.
        }
    }
}
