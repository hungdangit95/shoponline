using System.Collections.Generic;
using System.Linq;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class ProductCategoryRepository : EFRepository<ProductCategory, int>, IProductCategoryRepository
    {
        public readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(x => x.SeoAlias == alias).ToList();
        }
    }
}
