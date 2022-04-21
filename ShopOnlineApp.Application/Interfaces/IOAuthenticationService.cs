using Microsoft.AspNetCore.Identity;
using ShopOnlineApp.Application.ViewModels.Authentication;
using ShopOnlineApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IOAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationViewModel userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationViewModel userForAuth);
        Task<string> CreateToken();
    }
}
