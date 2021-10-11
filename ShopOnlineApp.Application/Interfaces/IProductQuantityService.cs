using System.Threading.Tasks;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IProductQuantityService
    {
        Task<bool> SellProduct(int productId, int quantity);
        Task UpdateQuantityProduct(int productId, int quantity);
    }
}
