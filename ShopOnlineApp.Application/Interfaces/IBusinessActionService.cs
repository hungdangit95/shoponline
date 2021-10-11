using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.BusinessAction;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBusinessActionService
    {
        Task<List<BusinessActionViewModel>> GetAll(BusinessActionRequest request);
        Task<List<BusinessActionViewModel>> GetByBusinessIds(string businessId);
        Task<BusinessActionViewModel> GetByActionId(int id);
        Task Update(BusinessActionViewModel businessVm);


    }
}
