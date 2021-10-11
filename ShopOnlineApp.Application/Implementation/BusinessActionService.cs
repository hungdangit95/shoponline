using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Business;
using ShopOnlineApp.Application.ViewModels.BusinessAction;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class BusinessActionService : IBusinessActionService
    {
        #region Private property
        private readonly IBusinessActionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor
        public BusinessActionService(IBusinessActionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public method
        public async Task<List<BusinessActionViewModel>> GetAll(BusinessActionRequest request)
        {
            return new BusinessActionViewModel().Map(await _repository.FindAll(x => x.BusinessId == request.BusinessId)).ToList();
        }

        public async Task<List<BusinessActionViewModel>> GetByBusinessIds(string businessId)
        {
            var items = (await _repository.FindAll(x => x.BusinessId == businessId)).GroupBy(x => x.Name).Select(x => x.FirstOrDefault());

            return new BusinessActionViewModel().Map(items).ToList();
        }

        public async Task<BusinessActionViewModel> GetByActionId(int id)
        {
            return new BusinessActionViewModel().Map(await _repository.FindById(id));
        }

        public async Task Update(BusinessActionViewModel businessVm)
        {
            var currentBusiness = await _repository.FindById(businessVm.Id);

            currentBusiness.Name = businessVm.Name;
            currentBusiness.Description = businessVm.Description;
            await _repository.Update(currentBusiness);
            _unitOfWork.Commit();
        }

        #endregion

    }
}
