using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.Business
{
    public class BusinessRequest:BaseRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
