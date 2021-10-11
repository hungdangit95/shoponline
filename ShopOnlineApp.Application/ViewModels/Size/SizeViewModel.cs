using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.Size
{
    public class SizeViewModel:ViewModelBase<SizeViewModel,Data.Entities.Size>
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
