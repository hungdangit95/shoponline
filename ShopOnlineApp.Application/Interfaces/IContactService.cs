using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Contact;
using ShopOnlineApp.Data.EF.Common;
namespace ShopOnlineApp.Application.Interfaces
{
    public interface IContactService
    {
        Task Add(ContactViewModel contactVm);
        Task Update(ContactViewModel contactVm);

        Task Delete(string id);

        Task<List<ContactViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<ContactViewModel>>> GetAllPaging(ContactRequest request);
        Task<ContactViewModel> GetById(string id);
        void SaveChanges();
    }
}
