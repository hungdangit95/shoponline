using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ShopOnlineApp.Data.Entities;
namespace ShopOnlineApp.Helper
{
    public class ShopOnlineClaimsPrincipalFactory: UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        public ShopOnlineClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }
        protected  override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            //get role user
            var roles = await UserManager.GetRolesAsync(user);

            identity.AddClaims(new Claim[]
            {
                new Claim("Email",user.Email??string.Empty),
                new Claim("FullName",user.FullName??string.Empty),
                new Claim("Avatar",user.Avatar??string.Empty),
                new Claim("TelephoneNumber",user.PhoneNumber ?? string.Empty),
                new Claim("Role", string.Join(";",roles)),
                new Claim("UserId",user.Id.ToString()), 

            });
            return identity;
        }

    }
}
