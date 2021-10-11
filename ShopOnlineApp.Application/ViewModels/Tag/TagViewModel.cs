using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.Tag
{
    public class TagViewModel:ViewModelBase<Data.Entities.Tag,TagViewModel>
    {
        public string Id { set; get; }

        public string Name { set; get; }

        public string Type { set; get; }
    }
}
