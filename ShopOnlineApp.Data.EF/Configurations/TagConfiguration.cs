using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnlineApp.Data.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Data.EF.Configurations
{
    public class TagConfiguration : DbEntityConfiguration<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(50)
                .IsRequired().HasColumnType("varchar(50)");
        }
    }
}
