using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
     public  class BlogCommentRepository: EFRepository<BlogComment, int>, IBlogCommentRepository
    {
        public BlogCommentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
