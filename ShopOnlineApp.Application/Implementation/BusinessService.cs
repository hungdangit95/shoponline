using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Business;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public BusinessService(IBusinessRepository businessRepository, IUnitOfWork unitOfWork)
        {
            _repository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BusinessViewModel>> GetAll()
        {
            return new BusinessViewModel().Map(await _repository.FindAll()).ToList();
        }

        public async Task<BaseReponse<ModelListResult<BusinessViewModel>>> GetAllPagingAsync(BusinessRequest request)
        {
            var query = await _repository.FindAll();

            if (!string.IsNullOrEmpty(request?.SearchText))
            {
                query = query.Where(x => x.Id.Contains(request.SearchText)
                                         || x.Name.Contains(request.SearchText));
            }

            int totalRow = await query.CountAsync();

            if (request != null)
                query = query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize);

            var items = new BusinessViewModel().Map(query).ToList();

            var businessName = items.Select(x => x.Id).ToList();

            List<string> data = new List<string>()
            {
                "HomeController",
                "BaseController",
                "LoginController",
                "LogoutController"
            };

            var businnesDiff = businessName.Except(data);

            List<BusinessViewModel> businessVMs = new List<BusinessViewModel>();

            foreach (var item in businnesDiff)
            {
                businessVMs.Add(items.SingleOrDefault(x => x.Id == item));
            }

            var result = new BaseReponse<ModelListResult<BusinessViewModel>>
            {
                Data = new ModelListResult<BusinessViewModel>()
                {
                    Items = businessVMs,
                    Message = Message.Success,
                    RowCount = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status = (int)QueryStatus.Success
            };

            return result;
        }

        public async Task<BusinessViewModel> GetByIdAsync(string id)
        {
            return new BusinessViewModel().Map(await _repository.FindById(id));
        }

        public async Task Update(BusinessViewModel businessVm)
        {
            var currentBusiness = await _repository.FindById(businessVm.Id);
            currentBusiness.Name = businessVm.Name;
            await _repository.Update(currentBusiness);
            _unitOfWork.Commit();
        }

        public async Task Delete(string id)
        {
            await _repository.Remove(id);
            _unitOfWork.Commit();
        }
    }
}
