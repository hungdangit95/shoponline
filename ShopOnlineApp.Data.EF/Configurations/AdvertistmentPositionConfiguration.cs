using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.EF.Extensions;

namespace ShopOnlineApp.Data.EF.Configurations
{
    public class AdvertistmentPositionConfiguration : DbEntityConfiguration<AdvertistmentPosition>
    {
        public override void Configure(EntityTypeBuilder<AdvertistmentPosition> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(20).IsRequired();
            // etc.
        }
    }
}
