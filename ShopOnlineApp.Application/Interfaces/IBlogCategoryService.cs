using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBlogCategoryService
    {
        Task<BlogCategoryViewModel> Add(BlogCategoryViewModel BlogCategoryVm);
        Task Update(BlogCategoryViewModel BlogCategoryVm);
        Task Delete(int id);
        Task<List<BlogCategoryViewModel>> GetAll();
        Task<List<BlogCategoryViewModel>> GetAll(string keyword);
        Task<List<BlogCategoryViewModel>> GetAllByParentId(int parentId);
        Task<BlogCategoryViewModel> GetById(int id);
        Task UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        Task ReOrder(int sourceId, int targetId);
       // Task<List<BlogCategoryViewModel>> GetHomeCategories(int top);
        // Task<List<BlogCategoryViewModel>> Unflatern();
        void Save();
    }
}
