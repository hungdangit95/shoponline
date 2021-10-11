using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Business;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBusinessService
    {
        Task<List<BusinessViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<BusinessViewModel>>> GetAllPagingAsync(BusinessRequest request);
        Task<BusinessViewModel>  GetByIdAsync(string id);
        Task Update(BusinessViewModel businessVm);
        Task Delete(string id);

    }
}
