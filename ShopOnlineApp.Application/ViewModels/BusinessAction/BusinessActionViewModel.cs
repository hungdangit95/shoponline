using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.BusinessAction
{
    public class BusinessActionViewModel:ViewModelBase<BusinessActionViewModel,Data.Entities.BusinessAction>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BusinessId { get; set; }
    }
}
