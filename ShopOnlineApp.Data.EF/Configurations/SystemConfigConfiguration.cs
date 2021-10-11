using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.EF.Extensions;

namespace ShopOnlineApp.Data.EF.Configurations
{
    class SystemConfigConfiguration : DbEntityConfiguration<SystemConfig>
    {
        public override void Configure(EntityTypeBuilder<SystemConfig> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            // etc.
        }
    }
}
