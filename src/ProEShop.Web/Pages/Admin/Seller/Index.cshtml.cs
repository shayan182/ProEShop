using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProEShop.Common;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Web.Pages.Admin.Seller;

public class IndexModel : PageBase
{
    #region Ctor

    private readonly ISellerService _sellerService;
    private readonly IProvinceAndCityService _provinceAndCityService;

    public IndexModel(ISellerService sellerService
        ,IProvinceAndCityService provinceAndCity)
    {
        _sellerService = sellerService;
        _provinceAndCityService = provinceAndCity;
    }

    #endregion
    [BindProperty(SupportsGet = true)] 
    public ShowSellersViewModel Sellers { get; set; }
        = new();

    public async Task OnGet()
    {
        var provinces = await _provinceAndCityService.GetProvincesToShowInSelectBoxAsync();
        Sellers.SearchSellers.Provinces = provinces.CreateSelectListItem();

        var cities = await _provinceAndCityService.GetCitiesToShowInSelectBoxAsync();
        Sellers.SearchSellers.Cities = cities.CreateSelectListItem();
    }
    public async Task<IActionResult> OnGetGetDataTable()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        return Partial("List", await _sellerService.GetSellers(Sellers));
    }
    public async Task<IActionResult> OnGetGetCities(long provinceId)
    {
        Dictionary<long, string> cities;
        if (provinceId == 0)
        {
            cities = await _provinceAndCityService.GetCitiesToShowInSelectBoxAsync();
            return Json(new JsonResultOperation(true, String.Empty)
            {
                Data = cities
            });
        }

        if (provinceId < 0)
            return Json(new JsonResultOperation(false, "لطفااستان مورد نظر را به درستی وارد نمایید "));

        if (!await _provinceAndCityService.IsExistsBy(nameof(Entities.ProvinceAndCity.Id), provinceId))
            return Json(new JsonResultOperation(false, "استان مورد نظر یافت نشد."));

        cities = await _provinceAndCityService.GetCitiesByProvinceIdToShowInSelectBoxAsync(provinceId);
        return Json(new JsonResultOperation(true, String.Empty)
        {
            Data = cities
        });
    }
    
    public async Task<IActionResult> OnGetGetSellerDetails(long sellerId)
    {
        var seller = await _sellerService.GetSellerDetails(sellerId);
        if (seller is null)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        }
        return Partial("SellerDetails", seller);
    }
}