using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolKit;
using ProEShop.Entities.Identity;
using ProEShop.IocConfig;
using ProEShop.Services.Contracts.Identity;
using ProEShop.ViewModels.Identity;
using ProEShop.ViewModels.Identity.Settings;

namespace ProEShop.Web.Pages.Identity;

public class RegisterLoginModel : PageBase
{
    private readonly IApplicationUserManager _userManager;
    private readonly ILogger<RegisterLoginModel> _logger;
    private readonly SiteSettings _siteSettings;

    public RegisterLoginModel(IApplicationUserManager userManager, 
        ILogger<RegisterLoginModel> logger,
        IOptionsMonitor<SiteSettings> siteSettings)
    {
        _userManager = userManager;
        _logger = logger;
        _siteSettings = siteSettings.CurrentValue;
    }

    public RegisterLoginViewModel RegisterLogin { get; set;}
    public void OnGet()
    {
        
    }
    public async Task<IActionResult> OnPostAsync(RegisterLoginViewModel registerLogin)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty,PublicConstantStrings.ModelStateErrorMessage);
            return Page();
        }
        var isInputEmail = registerLogin.PhoneNumberOrEmail.IsEmail();
        if (!isInputEmail) 
        {
            var addNewUser = false;
            var user = await _userManager.FindByNameAsync(registerLogin.PhoneNumberOrEmail);
            if (user is null)
            {
                user = new User()
                {
                    UserName = registerLogin.PhoneNumberOrEmail,
                    PhoneNumber = registerLogin.PhoneNumberOrEmail,
                    Avatar = _siteSettings.UserDefaultAvatar,
                    Email = $"{StringHelpers.GenerateGuid()}@test.com",
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation(LogCodes.RegisterCode, $"User {user.UserName} created a new account with phone number");
                    addNewUser = true;
                }
                else
                {
                    ModelState.AddErrorFromResult(result);
                    return Page();
                }
            }

            if (DateTime.Now > user.SendSmsLastTime.AddMinutes(3) || addNewUser)
            {
                var code = _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                //Todo send SMS
                user.SendSmsLastTime = DateTime.Now;
                await _userManager.UpdateAsync(user);
            }
        }
        return RedirectToPage("./LoginWithPhoneNumber", new { phoneNumber = registerLogin.PhoneNumberOrEmail });
    }
}
