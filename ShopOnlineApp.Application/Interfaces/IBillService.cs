using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Bill;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Application.ViewModels.Size;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Enums;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBillService
    {
        Task<BillViewModel> Create(BillViewModel billVm);
        Task Update(BillViewModel billVm);
        Task<BaseReponse<ModelListResult<BillViewModel>>> GetAllPaging(BillRequest request);

        Task<IEnumerable<BillViewModel>> GetOrdersByCustomer(Guid customerId);

        Task<BillViewModel> GetDetail(int billId);
        Task<BillDetailViewModel> CreateDetail(BillDetailViewModel billDetailVm);

        Task DeleteDetail(int productId, int billId, int colorId, int sizeId);

        Task UpdateStatus(int orderId, BillStatus status);
        Task<List<BillDetailViewModel>>  GetBillDetails(int billId);
        Task<List<ColorViewModel>>  GetColors();
        Task<List<SizeViewModel>>  GetSizes();
        Task<ColorViewModel> GetColor(int id);
        Task<SizeViewModel> GetSize(int id);
        void Save();
    }
}
