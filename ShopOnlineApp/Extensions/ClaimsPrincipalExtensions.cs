using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineApp.Extensions
{
    public class ClaimsPrincipalExtensions
    {
        public static CurrentUser GetCurrentUserDetails(this ClaimsPrincipal principal)
        {
            if (!principal.Claims.Any())
                return null;
            return new CurrentUser
            {
                Name = principal.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c =>
                c.Value).SingleOrDefault(),
                Email = principal.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c =>
                c.Value).SingleOrDefault(),
                Roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c =>
                c.Value).ToArray(),
                IsActive = Boolean.Parse(principal.Claims.Where(c => c.Type == "IsActive").
            Select(c => c.Value).SingleOrDefault()),
            };
        }
    }
}
