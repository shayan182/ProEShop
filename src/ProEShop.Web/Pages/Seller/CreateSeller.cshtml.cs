using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProEShop.Services.Contracts.Identity;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Web.Pages.Seller
{
    public class CreateSellerModel : PageModel
    {
        private readonly IApplicationUserManager _userManager;

        public CreateSellerModel(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }
        [BindProperty]
        public CreateSellerViewModel CreateSeller { get; set; }
        public async Task<IActionResult> OnGet(string phoneNumber)
        {
            if (!await _userManager.CheckForUserIsSeller(phoneNumber))
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public void OnPost()
        {
            //await _signInManager.SignInAsync(user, true);
        }
    }
}
