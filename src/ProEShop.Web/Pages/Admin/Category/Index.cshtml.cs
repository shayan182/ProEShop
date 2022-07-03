using Microsoft.AspNetCore.Mvc;
using ProEShop.Common;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Categories;

namespace ProEShop.Web.Pages.Admin.Category;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _uow;
    public IndexModel(ICategoryService categoryService,IUnitOfWork uow)
    {
        _categoryService = categoryService;
        _uow = uow;
    }

    #endregion


    public SearchCategoryViewModel SearchCategories { get; set; }
    = new();

    public void OnGet()
    {
    }
    public async Task<IActionResult> OnGetGetDataTable(SearchCategoryViewModel searchCategories)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        return Partial("List", await _categoryService.GetCategories(searchCategories));
    }
    public IActionResult OnGetAdd()
    {
        var model = new AddCategoryViewModel()
        {
            MainCategories = _categoryService.GetCategoriesToShowInSelelctBox().CreateSelectListItem(firstItemText: "خودش دسته اصلی باشد")
        };
        return Partial("Add", model);
    }
    public async Task<IActionResult> OnPostAdd(AddCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        var category = new Entities.Category
        {
            Description = model.Description,
            ShowInMenus = model.ShowInMenus,
            Title = model.Title,
            Slug = model.Slug,
            ParentId = model.ParentId == 0 ? null : model.ParentId,
            Picture = " "
        };
        var result = await _categoryService.AddAsync(category);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<AddCategoryViewModel>()
            });
        }
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت اضافه شد."));
    }
}