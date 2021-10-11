using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.BlogComment;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBlogCommentService
    {
       Task<BlogCommentViewModel>  Add(BlogCommentViewModel product);
       Task<BaseReponse<ModelListResult<BlogCommentViewModel>>>  GetAllPaging(BlogCommentRequest request);
    }
}
