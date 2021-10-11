using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.EF.Extensions;

namespace ShopOnlineApp.Data.EF.Configurations
{
    public class ContactDetailConfiguration : DbEntityConfiguration<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            // etc.
        }
    }
}
