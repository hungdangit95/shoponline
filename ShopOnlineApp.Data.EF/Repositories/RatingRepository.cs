using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class RatingRepository : EFRepository<Rating, int>, IRatingRepository
    {
        public RatingRepository(AppDbContext context) : base(context)
        {
        }
    }
}
