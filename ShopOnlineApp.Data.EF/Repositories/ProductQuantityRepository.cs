using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class ProductQuantityRepository : EFRepository<ProductQuantity, int>, IProductQuantityRepository
    {
        private readonly AppDbContext _context;
        public ProductQuantityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ProductQuantity>  GetByProductId(int productId)
        {
            return await _context.Set<ProductQuantity>().Where(x=>x.ProductId==productId).FirstOrDefaultAsync();
        }
    }
}
