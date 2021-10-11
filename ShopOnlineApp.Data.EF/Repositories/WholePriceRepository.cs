using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class WholePriceRepository : EFRepository<WholePrice, int>, IWholePriceRepository
    {
        public WholePriceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
