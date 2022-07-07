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


    public ShowCategoriesViewModel categories { get; set; }
    = new();

    public void OnGet()
    {
    }
    public async Task<IActionResult> OnGetGetDataTable(ShowCategoriesViewModel categories)
    {
        Thread.Sleep(1000);
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        categories.Pagination.Take = 5;
        return Partial("List", await _categoryService.GetCategories(categories));
    }
    public async Task<IActionResult> OnGetAdd(long id =  0)
    {
        if (id > 0)
        {
            if(! await _categoryService.IsExistsByIdAsync(id))
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
        }
        var model = new AddCategoryViewModel()
        {
            ParentId = id,
            MainCategories = _categoryService.GetCategoriesToShowInSelelctBox()
            .CreateSelectListItem(firstItemText: "خودش دسته اصلی باشد")
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

    public async Task<IActionResult> OnGetEdit(long id)
    {
        var model = await _categoryService.GetForEdit(id);
        if (model is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        model.MainCategories = _categoryService.GetCategoriesToShowInSelelctBox()
           .CreateSelectListItem(firstItemText: "خودش دسته اصلی باشد");
        return Partial("Edit", model);
    }
    public async Task<IActionResult> OnPostEditAsync(EditCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        if (model.Id == model.ParentId)
            return Json(new JsonResultOperation(false, "یک رکورد نمی تواند والد خودش باشد"));

        string pictureFileName = null;
        if (model.Picture.IsFileUploded())
        {
            pictureFileName = model.Picture.GenerateFileName();
        }

        var category = await _categoryService.FindByIdAsync(model.Id);
        if (category == null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        
        var oldFileName = category.Picture;

        category.Title = model.Title;
        category.Description = model.Description;
        category.ShowInMenus = model.ShowInMenus;
        category.Slug = model.Slug;
        category.ParentId = model.ParentId == 0 ? null : model.ParentId;
        category.Picture = pictureFileName;

        var result = await _categoryService.Update(category);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<AddCategoryViewModel>()
            });
        }
        await _uow.SaveChangesAsync();
        await _uploadFileService.SaveFile(model.Picture, pictureFileName,oldFileName, "images", "Categories");
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت اضافه شد."));
    }
    public async Task<IActionResult> OnPostDeleteAsync(long elementId)
    {
        Thread.Sleep(1000);

        var category = await _categoryService.FindByIdAsync(elementId);
        if (category is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        _categoryService.SoftDelete(category);
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت اضافه شد."));
    } 
    public async Task<IActionResult> OnPostDeletePicture(long elementId)
    {
        var category = await _categoryService.FindByIdAsync(elementId);
        if (category is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var fileName = category.Picture;ggggggggggg
        category.Picture = null;
        await _uow.SaveChangesAsync();
        _uploadFileService.DeleteFile(fileName, "images", "Categories");
        return Json(new JsonResultOperation(true, "تصویر دسته بندی با موفقیت حذف شد."));

    }
}