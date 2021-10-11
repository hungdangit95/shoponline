using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.Color
{
    public class ColorViewModel:ViewModelBase<ColorViewModel,Data.Entities.Color>
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name
        {
            get; set;
        }

        [StringLength(250)]
        public string Code { get; set; }
    }
}
