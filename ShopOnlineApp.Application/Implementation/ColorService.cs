using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class ColorService : IColorService
    {
        #region private method
        private readonly IColorRepository _colorRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constuctor
        public ColorService(IColorRepository colorRepository,
            IUnitOfWork unitOfWork)
        {
            _colorRepository = colorRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion

        public async Task Add(ColorViewModel pageVm)
        {
            var page = new ColorViewModel().Map(pageVm);
            await _colorRepository.Add(page);
        }

        public async Task Delete(int id)
        {
            await _colorRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<ColorViewModel>> GetAll()
        {
            return new ColorViewModel().Map(await _colorRepository.FindAll()).ToList();
        }

        public async Task<BaseReponse<ModelListResult<ColorViewModel>>> GetAllPaging(ColorRequest request)
        {

            var query = await _colorRepository.FindAll();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((request.PageIndex) * request.PageSize)
                .Take(request.PageSize);

            var items = new ColorViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<ColorViewModel>>
            {
                Data = new ModelListResult<ColorViewModel>()
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

        public async Task<ColorViewModel> GetById(int id)
        {
            return new ColorViewModel().Map(await _colorRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public async Task Update(ColorViewModel pageVm)
        {
            var page = new ColorViewModel().Map(pageVm);
            await _colorRepository.Update(page);
        }
    }

}
