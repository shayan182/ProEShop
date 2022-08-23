using Microsoft.AspNetCore.Mvc;
using ProEShop.Common;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.Services.Services;
using ProEShop.ViewModels.Categories;

namespace ProEShop.Web.Pages.Admin.Category;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _uow;
    private readonly IUploadFileService _uploadFileService;
    private readonly IBrandService _brandServices;

    public IndexModel(ICategoryService categoryService,
        IUnitOfWork uow, IUploadFileService uploadFileService,
        IBrandService brandServices)
    {
        _categoryService = categoryService;
        _uploadFileService = uploadFileService;
        _brandServices = brandServices;
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
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        return Partial("List", await _categoryService.GetCategories(categories));
    }
    public async Task<IActionResult> OnGetAdd(long id = 0)
    {
        if (id > 0)
        {
            if (!await _categoryService.IsExistsBy(nameof(Entities.Category.Id), id))
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
        }

        var categories = await _categoryService.GetCategoriesToShowInSelectBoxAsync();
        var model = new AddCategoryViewModel()
        {
            ParentId = id,
            MainCategories = categories
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
        if (model.Picture.IsFileUploaded())
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
        await _uploadFileService.SaveFile(model.Picture, pictureFileName, null, "images", "Categories");
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت اضافه شد."));
    }

    public async Task<IActionResult> OnGetEdit(long id)
    {
        var model = await _categoryService.GetForEdit(id);
        if (model is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        var categories = await _categoryService.GetCategoriesToShowInSelectBoxAsync();
        model.MainCategories = categories
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
        {
            return Json(new JsonResultOperation(false, "یک رکورد نمی تواند والد خودش باشد")
            );

        }

        string pictureFileName = null;
        if (model.Picture.IsFileUploaded())
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
        category.Picture = pictureFileName == null ? oldFileName : pictureFileName;

        var result = await _categoryService.Update(category);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<AddCategoryViewModel>()
            });
        }
        await _uow.SaveChangesAsync();
        await _uploadFileService.SaveFile(model.Picture, pictureFileName, oldFileName, "images", "Categories");
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت اضافه شد."));
    }
    public async Task<IActionResult> OnPostDeleteAsync(long elementId)
    {

        var category = await _categoryService.FindByIdAsync(elementId);
        if (category is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        _categoryService.SoftDelete(category);
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت حذف شد."));
    }
    public async Task<IActionResult> OnPostDeletePicture(long elementId)
    {
        var category = await _categoryService.FindByIdAsync(elementId);
        if (category is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var fileName = category.Picture;
        category.Picture = null;
        await _uow.SaveChangesAsync();
        _uploadFileService.DeleteFile(fileName, "images", "Categories");
        return Json(new JsonResultOperation(true, "تصویر دسته بندی با موفقیت حذف شد."));
    }
    public async Task<IActionResult> OnPostRestorAsync(long elementId)
    {
        var category = await _categoryService.FindByIdAsync(elementId);
        if (category is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        _categoryService.Restore(category);
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت بازگردانی شد."));
    }

    public async Task<IActionResult> OnGetAddBrand(long selectedCategoryId)
    {
        var model = new AddBrandToCategoryViewModel
        {
            //چون نام آرگومان ورودی با یک یاز پراپرتی های داخل مودل یکی هست خودش اونو بایند میکنه
            SelectedBrands = await _categoryService.GetCategoryBrands(selectedCategoryId)
        };
        return Partial("AddBrand", model);
    }
    public async Task<IActionResult> OnPostAddBrandAsync(AddBrandToCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        if (model.SelectedCategoryId < 1)
            return Json(new JsonResultOperation(false));

        var selectedCategory = await _categoryService.FindByIdAsync(model.SelectedCategoryId);
        if (selectedCategory is null)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        }
        selectedCategory.CategoryBrands.Clear();

        model.SelectedBrands = model.SelectedBrands.Distinct().ToList();

        var brandIds = await _brandServices.GetBrandIdsByList(model.SelectedBrands);
        brandIds.ForEach(brandId => selectedCategory.CategoryBrands.Add(new CategoryBrand()
        {
            BrandId = brandId
        }));

        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت به دسته بندی اضافه شد."));

    }
    public async Task<IActionResult> OnPostCheckForTitle(string title)
    {
        return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Title), title));
    }

    public async Task<IActionResult> OnPostCheckForSlug(string slug)
    {
        return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Slug), slug));
    }

    public async Task<IActionResult> OnPostCheckForTitleOnEdit(string title, long id)
    {
        return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Title), title, id));
    }

    public async Task<IActionResult> OnPostCheckForSlugOnEdit(string slug, long id)
    {
        return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Slug), slug, id));
    }
    public async Task<IActionResult> OnGetAutocompleteSearch(string term)
    {
        return Json(await _brandServices.AutoCompleteSearch(term));
    }
}