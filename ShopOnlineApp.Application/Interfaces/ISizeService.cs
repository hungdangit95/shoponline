using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Size;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface ISizeService
    {
        Task Add(SizeViewModel contactVm);
        Task Update(SizeViewModel contactVm);
        Task Delete(int id);
        Task<List<SizeViewModel>>  GetAll();
        Task<BaseReponse<ModelListResult<SizeViewModel>>>  GetAllPaging(SizeRequest request);
        Task<SizeViewModel> GetById(int id);
        void SaveChanges();
    }
}
