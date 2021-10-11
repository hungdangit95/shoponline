using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.Footer
{
    public class FooterViewModel:ViewModelBase<Data.Entities.Footer,FooterViewModel>
    {
        public string Id { set; get; }
        public string Content { set; get; }
    }
}
