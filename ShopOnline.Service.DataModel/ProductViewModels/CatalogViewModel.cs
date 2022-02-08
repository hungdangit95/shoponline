using ShopOnlineApp.Application.ViewModels.Product;

namespace ShopOnlineApp.Models.ProductViewModels
{
    public class CatalogViewModel:FilterBase<ProductViewModel>
    {
        public ProductCategoryViewModel Category { set; get; }
    }

}
