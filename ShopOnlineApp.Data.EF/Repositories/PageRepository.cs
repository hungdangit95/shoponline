using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class PageRepository : EFRepository<Page, int>, IPageRepository
    {
        public PageRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
