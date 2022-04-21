using System.ComponentModel.DataAnnotations;
using ShopOnlineApp.Application.Common;
namespace ShopOnlineApp.Application.ViewModels.Authentication
{
    public class UserForAuthenticationViewModel 
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; init; }
    }
}
