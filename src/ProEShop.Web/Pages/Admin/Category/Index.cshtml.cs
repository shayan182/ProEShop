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
    private readonly IUploadFileService _uploadFileService;
    public IndexModel(ICategoryService categoryService,IUnitOfWork uow,IUploadFileService uploadFileService)
    {
        _categoryService = categoryService;
        _uploadFileService = uploadFileService;
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
        string pictureFileName = null;
        if(model.Picture.IsFileUploded())
        {
            pictureFileName = model.Picture.GenerateFileName();
        }
        var category = new Entities.Category
        {
            Description = model.Description,
            ShowInMenus = model.ShowInMenus,
            Title = model.Title,
            Slug = model.Slug,
            ParentId = model.ParentId == 0 ? null : model.ParentId,
            Picture = pictureFileName 
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
        await _uploadFileService.SaveFile(model.Picture, pictureFileName,"images","Categories");
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت اضافه شد."));
    }
}