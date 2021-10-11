using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryViewModel>   Add(ProductCategoryViewModel productCategoryVm);
        Task Update(ProductCategoryViewModel productCategoryVm);
        Task Delete(int id);
        Task<List<ProductCategoryViewModel>> GetAll();
        Task<List<ProductCategoryViewModel>> GetAll(string keyword);
        Task<List<ProductCategoryViewModel>> GetAllByParentId(int parentId);
        Task<ProductCategoryViewModel> GetById(int id);
        Task UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        Task ReOrder(int sourceId, int targetId);
        Task<List<ProductCategoryViewModel>> GetHomeCategories(int top);
        void Save();
    }
}
