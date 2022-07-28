using Microsoft.AspNetCore.Mvc;
using ProEShop.Services.Contracts;
using ProEShop.Services.Contracts.Identity;
using ProEShop.ViewModels.Sellers;
using ProEShop.Common;
using ProEShop.Common.Helpers;

namespace ProEShop.Web.Pages.Seller;

public class CreateSellerModel : PageBase
{
    private readonly ISellerService _sellerService;
    private readonly IApplicationUserManager _userManager;
    private readonly IProvinceAndCityService _provinceAndCityService;
    public CreateSellerModel(IApplicationUserManager userManager, IProvinceAndCityService provinceAndCityService, ISellerService sellerService)
    {
        _userManager = userManager;
        _provinceAndCityService = provinceAndCityService;
        _sellerService = sellerService;
    }

    [BindProperty]
    public CreateSellerViewModel CreateSeller { get; set; }
        = new();
    public async Task<IActionResult> OnGet(string phoneNumber)
    {
        if (!await _userManager.CheckForUserIsSeller(phoneNumber))
        {
            return RedirectToPage("/Error");
        }

        CreateSeller.PhoneNumber = phoneNumber;
        var provinces = await _provinceAndCityService.GetProvincesToShowInSelectBoxAsync();
        CreateSeller.Provinces = provinces.CreateSelectListItem();

        return Page();
    }

    public void OnPost()
    {
        //await _signInManager.SignInAsync(user, true);
    }

    public async Task<IActionResult> OnGetGetCities(long provinceId)
    {
        if (provinceId == 0)
        {
            return Json(new JsonResultOperation(true, string.Empty)
            {
                Data = new Dictionary<long, string>()
            });
        }

        if (provinceId < 0)
            return Json(new JsonResultOperation(true, "لطفااستان مورد نظر را به درستی وارد نمایید "));

        if (!await _provinceAndCityService.IsExistsBy(nameof(Entities.ProvinceAndCity.Id), provinceId))
            return Json(new JsonResultOperation(true, "استان مورد نظر یافت نشد."));

        var cities = await _provinceAndCityService.GetCitiesByProvinceIdToShowInSelectBoxAsync(provinceId);
        return Json(new JsonResultOperation(true, String.Empty)
        {
            Data = cities
        });
    }

    public async Task<IActionResult> OnGetCheckForShopName(CreateSellerViewModel createSeller)
    {
        return Json(!await _sellerService.IsExistsBy(nameof(Entities.Seller.ShopName),
            CreateSeller.ShopName));
    }
}