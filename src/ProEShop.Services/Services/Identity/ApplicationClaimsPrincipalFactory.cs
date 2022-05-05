using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProEShop.Common.Constants;
using ProEShop.Entities.Identity;

namespace ProEShop.Services.Services.Identity;

public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<User,Role>
{
    public ApplicationClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<IdentityOptions> options)
        : base(userManager, roleManager, options)
    {}

    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        // adds all `Options.ClaimsIdentity.RoleClaimType -> Role Claims` automatically +
        // `Options.ClaimsIdentity.UserIdClaimType -> userId`
        // & `Options.ClaimsIdentity.UserNameClaimType -> userName`
        var principal = await base.CreateAsync(user);
        AddCustomClaims(user,principal);
        return principal;
    }

    private static void AddCustomClaims(User user, ClaimsPrincipal principal)
    {
        (principal.Identity as ClaimsIdentity).AddClaims(new[]
        {
            new Claim(IdentityClaimNames.Avatar, user.Avatar ?? string.Empty, ClaimValueTypes.String),
            new Claim(IdentityClaimNames.FullName, user.FullName ?? string.Empty, ClaimValueTypes.String)
        });
    }
}