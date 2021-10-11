using System.Linq;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.BlogComment;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class BlogCommentService : IBlogCommentService
    {
        private readonly IBlogCommentRepository _blogCommentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BlogCommentService(IBlogCommentRepository blogCommentRepository, IUnitOfWork unitOfWork)
        {
            _blogCommentRepository = blogCommentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BlogCommentViewModel> Add(BlogCommentViewModel blogCommentVm)
        {
            var page = new BlogCommentViewModel().Map(blogCommentVm);
            await _blogCommentRepository.Add(page);
            _unitOfWork.Commit();
            return blogCommentVm;
        }

        public async Task<BaseReponse<ModelListResult<BlogCommentViewModel>>> GetAllPaging(BlogCommentRequest request)
        {
            var query = await _blogCommentRepository.FindAll();

            if (request.BlogId > 0)
            {
                query = query.Where(x => x.BlogId == request.BlogId);
            }

            int totalRow = query.Count();
            query = query.OrderByDescending(x => x.DateCreated)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize);

            var items = new BlogCommentViewModel().Map(query).ToList();

            return new BaseReponse<ModelListResult<BlogCommentViewModel>>
            {
                Data = new ModelListResult<BlogCommentViewModel>()
                {
                    Items = items,
                    Message = Message.Success,
                    RowCount = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status = (int)QueryStatus.Success
            };
        }
    }

}
