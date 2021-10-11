using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Blogs;
using ShopOnlineApp.Application.ViewModels.Tag;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBlogService
    {
        Task<BlogViewModel> Add(BlogViewModel product);
        Task Update(BlogViewModel product);
        Task Delete(int id);
        Task<List<BlogViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<BlogViewModel>>> GetAllPaging(BlogRequest request);
        Task<IEnumerable<BlogViewModel>> GetLastest(int top);
        Task<List<BlogViewModel>> GetHotProduct(int top);
        Task<List<BlogViewModel>> GetListPaging(int page, int pageSize, string sort);
        Task<List<BlogViewModel>> Search(string keyword, int page, int pageSize, string sort);
        Task<List<BlogViewModel>> GetList(string keyword);
        Task<List<BlogViewModel>> GetReatedBlogs(int id, int top);
        Task<List<string>>  GetListByName(string name);
        Task<BlogViewModel> GetById(int id);
        void Save();
        Task<List<TagViewModel>>  GetListTagById(int id);
        Task<TagViewModel>  GetTag(string tagId);
        Task IncreaseView(int id);
        Task<List<BlogViewModel>>  GetListByTag(string tagId, int page, int pagesize, out int totalRow);
        Task<List<TagViewModel>>  GetListTag(string searchText);
    }
}
