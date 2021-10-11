using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class BusinessActionRepository : EFRepository<BusinessAction, int>,IBusinessActionRepository
    {
        public BusinessActionRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
