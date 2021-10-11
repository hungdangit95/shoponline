using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface ISlideService
    {
        Task Add(SlideViewModel slideVm);
        Task Update(SlideViewModel slideVm);
        Task Delete(int id);
        Task<List<SlideViewModel>>  GetAll();
        Task<BaseReponse<ModelListResult<SlideViewModel>>>  GetAllPaging(SlideRequest request);
        Task<SlideViewModel>  GetById(int id);
        void SaveChanges();
    }
}
