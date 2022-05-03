using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProEShop.DataLayer.Context;
using ProEShop.Entities.Identitiy;
using ProEShop.Services.Contracts.Identity;

namespace ProEShop.Services.Implements.Identity;

public class ApplicationSignInManager : 
    SignInManager<User>, IApplicationSignInManager
{
    
    #region Custom Class



    #endregion

    public ApplicationSignInManager(
        IApplicationUserManager userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<User>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> confirmation)
        : base((UserManager<User>)userManager, contextAccessor, claimsFactory,
            optionsAccessor, logger, schemes, confirmation)
    {
    }
}