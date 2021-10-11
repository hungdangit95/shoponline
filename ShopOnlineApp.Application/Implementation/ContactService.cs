using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Contact;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class ContactService : IContactService
    {
        #region private method
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region constructor

        #region public method
        public ContactService(IContactRepository contactRepository,
           IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }


        #endregion


        public async Task Add(ContactViewModel pageVm)
        {
            var page = new ContactViewModel().Map(pageVm);
            await _contactRepository.Add(page);
        }

        public async Task Delete(string id)
        {
            await _contactRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<ContactViewModel>> GetAll()
        {
            return new ContactViewModel().Map(await _contactRepository.FindAll()).ToList();
        }

        public async Task<BaseReponse<ModelListResult<ContactViewModel>>> GetAllPaging(ContactRequest request)
        {

            var query = await _contactRepository.FindAll();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((request.PageIndex) * request.PageSize)
                .Take(request.PageSize);

            var items = new ContactViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<ContactViewModel>>
            {
                Data = new ModelListResult<ContactViewModel>()
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
        public async Task<ContactViewModel> GetById(string id)
        {
            return new ContactViewModel().Map(await _contactRepository.FindById(id));
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
        public async Task Update(ContactViewModel pageVm)
        {
            var page = new ContactViewModel().Map(pageVm);
            await _contactRepository.Update(page);
        }

        #endregion

    }
}
