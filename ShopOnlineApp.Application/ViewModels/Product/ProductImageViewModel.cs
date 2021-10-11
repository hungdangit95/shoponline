using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.ViewModels.Product
{
    public class ProductImageViewModel:ViewModelBase<ProductImage,ProductImageViewModel>
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        public string Path { get; set; }

        public string Caption { get; set; }
    }
}
