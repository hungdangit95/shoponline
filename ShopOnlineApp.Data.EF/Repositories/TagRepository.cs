using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class TagRepository : EFRepository<Tag, string>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {

        }

    }
}
