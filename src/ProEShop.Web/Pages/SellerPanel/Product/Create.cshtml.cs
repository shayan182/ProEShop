using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Helpers;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Product;

namespace ProEShop.Web.Pages.SellerPanel.Product;

public class CreateModel : SellerPanelBase
{
    #region Ctor

    private readonly ICategoryService _categoryService;

    public CreateModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    #endregion

    public AddProductViewModel Product { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetGetCategories(long[] selectedCategoriesIds)
    {
        var result = await _categoryService.GetCategoriesForCreateProduct(selectedCategoriesIds);
        return Partial("_SelectProductCategoryPartial", result);
    }

    public async Task<IActionResult> OnGetGetCategoryBrands(long categoryId)
    {
        if (categoryId < 1)
            return Json(new JsonResultOperation(false));

        var brands = await _categoryService.GetBrandsByCategoryId(categoryId);
        return Json(new JsonResultOperation(true,String.Empty)
        {
            Data = brands
        });
    }
}