using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Service.DataModel.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
