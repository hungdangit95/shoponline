using System;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Blogs;

namespace ShopOnlineApp.Application.ViewModels.BlogComment
{
    public class BlogCommentViewModel:ViewModelBase<Data.Entities.BlogComment,BlogCommentViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public int BlogId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BlogViewModel Blog { get; set; }
    }
}
