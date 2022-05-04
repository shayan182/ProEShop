using Microsoft.AspNetCore.Identity;

namespace ProEShop.ViewModels.Identity.Settings;
public class SiteSettings
{
    // User need to confirm email to login?
    public bool EnableEmailConfirmation { get; set; }

    //Expiration date the link confirm email , default = 1 day
    public TimeSpan EmailConfirmationTokenProviderLifespan { get; set; }
    public PasswordOptions PasswordOptions { get; set; }
    public LockoutOptions LockoutOptions { get; set; }
    public CookieOption CookieOptions { get; set; }


}
