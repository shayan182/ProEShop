using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Entities.Identity;
using ProEShop.IocConfig;
using ProEShop.Services.Contracts.Identity;
using ProEShop.ViewModels.Identity.Settings;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Web.Pages.Seller;

public class RegisterModel : PageModel
{
    #region Constructor

    private readonly SiteSettings _siteSettings;
    private readonly IApplicationUserManager _userManager;
    private readonly ILogger<RegisterModel> _logger;
    public RegisterModel(
        IApplicationUserManager userManager,
        IOptionsMonitor<SiteSettings> siteSettings,
        ILogger<RegisterModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _siteSettings = siteSettings.CurrentValue;
    }
    #endregion
    [BindProperty]
    public RegisterSellerViewModel RegisterSeller { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()// We use BindProperty, so that model bind here 
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Page();
        }

        var addNewUser = false;
        var user = await _userManager.FindByNameAsync(RegisterSeller.PhoneNumber);
        if (user is null)
        {
            user = new User()
            {
                UserName = RegisterSeller.PhoneNumber,
                PhoneNumber = RegisterSeller.PhoneNumber,
                Avatar = _siteSettings.UserDefaultAvatar,
                Email = RegisterSeller.Email
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

        return RedirectToPage("./ConfirmationPhoneNumber", new { phoneNumber = RegisterSeller.PhoneNumber });
    }
}