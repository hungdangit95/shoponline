using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IRatingService
    {
        Task<RatingViewModel>  Add(RatingViewModel product);
        Task<BaseReponse<ModelListResult<RatingViewModel>>>  GetAllPaging(RateRequest request);
    }
}
