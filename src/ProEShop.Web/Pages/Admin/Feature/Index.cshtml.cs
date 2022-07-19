using Microsoft.AspNetCore.Mvc;
using ProEShop.Common;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Features;

namespace ProEShop.Web.Pages.Admin.Feature;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly ICategoryFeatureService _categoryFeatureService;
    private readonly IFeatureService _featureService;
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _uow;



    #endregion

    public IndexModel(ICategoryFeatureService categoryFeatureService, IFeatureService featureService, ICategoryService categoryService, IUnitOfWork uow)
    {
        _categoryFeatureService = categoryFeatureService;
        _featureService = featureService;
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

        var x = await _featureService.GetCategoryFeatures(features);
        return Partial("List", await _featureService.GetCategoryFeatures(features));
    }

    public async Task<IActionResult> OnPostDelete(int categoryId, int featureId)
    {
        var category = await _categoryFeatureService.GetCategoryFeature(categoryId, featureId);
        if (category is not null)
        {
            _categoryFeatureService.Remove(category);
            await _uow.SaveChangesAsync();
        }
        return Json(new JsonResultOperation(true, "ویژگی دسته بندی مورد نظر با موفقیت حذف شد!"));
    }
    public async Task<IActionResult> OnGetAdd()
    {
        var categories = await _categoryService.GetCategoriesToShowInSelectBoxAsync();
        var model = new AddFeatureViewModel()
        {
            Categories = categories
            .CreateSelectListItem()
        };
        return Partial("Add", model);
    }
    public async Task<IActionResult> OnPostAddAsync(AddFeatureViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var searchedTitle = model.Title.Trim();
        var feature = await _featureService.FindByTitleAsync(searchedTitle);
        if (feature is null)
        {
            await _featureService.AddAsync(new Entities.Feature()
            {
                Title = searchedTitle,
                categoryFeatures = new List<CategoryFeature>()
                {
                    new CategoryFeature()
                    {
                        CategoryId = model.CategoryId,
                    }
                }
            });
        }
        else
        {
            var categoryFeature = _categoryFeatureService.GetCategoryFeature(model.CategoryId, feature.Id);
            if (categoryFeature is null)
            {
                feature.categoryFeatures.Add(new CategoryFeature()
                {
                    CategoryId = model.CategoryId
                });
            }
        }

        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "ویژگی دسته بندی مورد نظر با موفقیت اضافه شد!"));
    }

    public async Task<IActionResult> OnGetAutocompleteSearch(string term)
    {
        return Json(await _featureService.AutoCompleteSearch(term));
    }
}