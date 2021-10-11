using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.Tag;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        #region method
        Task<List<ProductViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<ProductViewModel>>> GetAllPaging(ProductRequest request);
        Task<ModelListResult<ProductFullViewModel>> FilterProducts(ProductRequest request);
        Task<ProductViewModel> Add(ProductViewModel product);
        Task<ProductViewModel> GetById(int id);
        Task Update(ProductViewModel product);
        void Save();
        Task Delete(int id);
        Task ImportExcel(string filePath, int categoryId);
        Task AddQuantity(int productId, List<ProductQuantityViewModel> quantities);
        Task<List<ProductQuantityViewModel>> GetQuantities(int productId);
        Task AddImages(int productId, string[] images);
        Task<List<ProductImageViewModel>> GetImages(int productId);
        Task AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);
        Task<List<WholePriceViewModel>> GetWholePrices(int productId);
        Task<List<ProductViewModel>> GetLastest(int top);
        Task<List<ProductViewModel>> GetHotProduct(int top);
        Task<List<ProductViewModel>> GetRelatedProducts(int id, int top);
        Task<List<ProductViewModel>> GetRatingProducts(int top);
        Task<List<ProductViewModel>> GetUpsellProducts(int top);
        Task<List<TagViewModel>> GetProductTags(int productId);
        Task<bool> CheckAvailability(int productId, int size, int color);
        Task<IEnumerable<ProductViewModel>> SearchAsync(string key,int page,int pageSize=5);
        #endregion
    }
}
