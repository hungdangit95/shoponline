using System.Collections.Generic;
using System.Linq;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class BlogCategoryRepository : EFRepository<BlogCategory, int>, IBlogCategoryRepository
    {
        public BlogCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public List<BlogCategory> GetByAlias(string alias)
        {
            return _context.BlogCategories.Where(x => x.SeoAlias == alias).ToList();
        }
    }
}
