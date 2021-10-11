using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.BlogTag
{
    public class BlogTagViewModel:ViewModelBase<Data.Entities.BlogTag,BlogTagViewModel>
    {
        public int BlogId { set; get; }
        public string TagId { set; get; }
    }
}
