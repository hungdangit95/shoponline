using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Function;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        Task Add(FunctionViewModel function);
        Task<List<FunctionViewModel>> GetAll(string filter);
        Task<List<FunctionViewModel>> GetFunctionByRoles(FunctionRequest reques);
        Task<IEnumerable<FunctionViewModel>>  GetAllWithParentId(string parentId);
        Task<FunctionViewModel>  GetById(string id);
        Task Update(FunctionViewModel function);
        Task Delete(string id);
        void Save();
        Task<bool>  CheckExistedId(string id);
        Task UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items);
        Task ReOrder(string sourceId, string targetId);
    }
}
