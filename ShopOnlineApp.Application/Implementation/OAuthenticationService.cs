using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Authentication;
using ShopOnlineApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineApp.Application.Implementation
{
    public class OAuthenticationService : IOAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        private AppUser _user;
        public OAuthenticationService(ILogger<AuthenticationService> logger, UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        public async Task<IdentityResult> RegisterUser(UserForRegistrationViewModel userForRegistration)
        {
            var user = userForRegistration.Map(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (result.Succeeded)
            {
                //var isExist = await _roleManager.RoleExistsAsync()
                foreach (var role in userForRegistration.Roles)
                {
                    var isExist = await _roleManager.RoleExistsAsync(role);
                    if (!isExist)
                    {
                        break;
                    }
                }
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            }
            return result;
        }


        public async Task<bool> ValidateUser(UserForAuthenticationViewModel userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user,
            userForAuth.Password));
            if (!result)
                _logger.LogWarning($"{nameof(ValidateUser)}: Authentication failed. Wrong username or password.");
            return result;
        }
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Base64UrlEncoder.DecodeBytes("sddddddddwewewewwwwwwwwwwwwwwwwrerevvasasaawqw");
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, _user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}
