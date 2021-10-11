using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Page;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IPageService : IDisposable
    {
        Task Add(PageViewModel pageVm);
        Task Update(PageViewModel pageVm);
        Task Delete(int id);
        Task<List<PageViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<PageViewModel>>> GetAllPaging(PageRequest request);
        Task<PageViewModel> GetByAlias(string alias);
        Task<PageViewModel> GetById(int id);
        void SaveChanges();

    }
}
