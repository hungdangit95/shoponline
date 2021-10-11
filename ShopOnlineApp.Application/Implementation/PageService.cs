using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Page;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PageService(IPageRepository pageRepository,
            IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(PageViewModel pageVm)
        {
            var page =new PageViewModel().Map(pageVm);
           await _pageRepository.Add(page);
        }

        public async Task Delete(int id)
        {
           await _pageRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<PageViewModel>>  GetAll()
        {
            return new PageViewModel().Map(await _pageRepository.FindAll()).ToList();
        }

        public async Task<BaseReponse<ModelListResult<PageViewModel>>>  GetAllPaging(PageRequest request)
        {
            var query =await _pageRepository.FindAll();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText));

            int totalRow = query.Count();

            var data = query.OrderByDescending(x => x.Alias)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize);

            var items = new PageViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<PageViewModel>>()
            {
                Data = new ModelListResult<PageViewModel>()
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
        public async Task<PageViewModel>  GetByAlias(string alias)
        {
            return new PageViewModel().Map(await _pageRepository.FindSingle(x => x.Alias == alias));
        }

        public async Task<PageViewModel>  GetById(int id)
        {
            return new PageViewModel().Map(await _pageRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public async Task Update(PageViewModel pageVm)
        {
            var page =new  PageViewModel().Map(pageVm);
           await _pageRepository.Update(page);
        }
    }
}
