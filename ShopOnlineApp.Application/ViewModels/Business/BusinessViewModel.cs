using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.Business
{
    public class BusinessViewModel:ViewModelBase<Data.Entities.Business,BusinessViewModel>
    {
        public string Id { get; set; }

        public  string Name { get; set; }
    }
}
