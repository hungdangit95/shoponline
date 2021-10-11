using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Data.EF.Common;
namespace ShopOnlineApp.Application.Interfaces
{
    public interface IColorService
    {
        Task Add(ColorViewModel contactVm);
        Task Update(ColorViewModel contactVm);
        Task Delete(int id);
        Task<List<ColorViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<ColorViewModel>>> GetAllPaging(ColorRequest request);
        Task<ColorViewModel> GetById(int id);
        void SaveChanges();
    }
}
