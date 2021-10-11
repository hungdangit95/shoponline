using System.Collections.Generic;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.ViewModels.Function
{
    public class FunctionRequest:BaseRequest
    {
        public List<string> Roles { get; set; }
    }
}
