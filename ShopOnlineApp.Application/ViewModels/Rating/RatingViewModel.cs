using System;
using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;

namespace ShopOnlineApp.Application.ViewModels.Rating
{
    public class RatingViewModel:ViewModelBase<Data.Entities.Rating,RatingViewModel>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Value { get; set; }
        public string NickName { get; set; }
        public string Summary { get; set; }
        public string Review { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
