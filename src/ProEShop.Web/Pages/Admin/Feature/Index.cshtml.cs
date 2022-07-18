using Microsoft.AspNetCore.Mvc;
using ProEShop.Common;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Features;

namespace ProEShop.Web.Pages.Admin.Feature;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly ICategoryFeatureService _categoryFeatureService;
    private readonly IFeatureService _FeatureService;
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _uow;

    

    #endregion

    public IndexModel(ICategoryFeatureService categoryFeatureService, IFeatureService featureService, ICategoryService categoryService, IUnitOfWork uow)
    {
        _categoryFeatureService = categoryFeatureService;
        _FeatureService = featureService;
        _categoryService = categoryService;
        _uow = uow;
    }

    public ShowFeaturesViewModel Features { get; set; }
        = new();
    public async Task OnGet()
    {
        var categories = await _categoryService.GetCategoriesToShowInSelectBoxAsync();
        Features.SearchFeatures.Categories = categories.CreateSelectListItem();
    }

    public async Task<IActionResult> OnGetGetDataTableAsync(ShowFeaturesViewModel features)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var x = await _FeatureService.GetCategoryFeatures(features);
        return Partial("List", await _FeatureService.GetCategoryFeatures(features));
    }

    public async Task<IActionResult> OnPostDelete(int categoryId,int featureId)
    {
        var category = await _categoryFeatureService.GetCategoryFeatureToRemove(categoryId, featureId);
        if (category is not null)
        { 
            _categoryFeatureService.Remove(category);
            await _uow.SaveChangesAsync();
        }
        return Json(new JsonResultOperation(true, "ویژگی دسته بندی مورد نظر با موفقیت حذف شد!"));
    }
}