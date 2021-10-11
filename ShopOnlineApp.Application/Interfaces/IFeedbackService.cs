using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Feedback;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task Add(FeedbackViewModel feedbackVm);

        Task Update(FeedbackViewModel feedbackVm);

        Task Delete(int id);

        Task<List<FeedbackViewModel>>  GetAll();
        Task<BaseReponse<ModelListResult<FeedbackViewModel>>>  GetAllPaging(FeedbackRequest request);
        Task<FeedbackViewModel>  GetById(int id);
        void SaveChanges();
    }
}
