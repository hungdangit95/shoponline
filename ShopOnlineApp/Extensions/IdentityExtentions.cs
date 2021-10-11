using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopOnlineApp.Extensions
{
    public static class IdentityExtentions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x => x.Type == "UserId");
            return Guid.Parse(claim.Value);
        }
        public static string GetSpecificDefault(this ClaimsPrincipal claimPrincipal, string typeClaim)
        {
            var claim = claimPrincipal.Claims.FirstOrDefault(x => x.Type == typeClaim);
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}