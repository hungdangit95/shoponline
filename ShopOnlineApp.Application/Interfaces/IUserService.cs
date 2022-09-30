using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.User;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userVm);

        Task DeleteAsync(string id);

        Task<List<AppUserViewModel>> GetAllAsync();

        Task<BaseReponse<ModelListResult<AppUserViewModel>>> GetAllPagingAsync(UserRequest request);

        Task<AppUserViewModel> GetById(string id);


        Task UpdateAsync(AppUserViewModel userVm);

        string GetUserId();

        void SaveChanges();

    }
}
