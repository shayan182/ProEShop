using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.Services.Services;
using ProEShop.ViewModels.Brands;
using ProEShop.ViewModels.Variants;

namespace ProEShop.Web.Pages.Admin.Variant;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly IVariantService _variantService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public IndexModel(
        IVariantService variantService, IMapper mapper, IUnitOfWork uow)
    {
        _variantService = variantService;
        _mapper = mapper;
        _uow = uow;
    }

    #endregion

    [BindProperty(SupportsGet = true)]
    public ShowVariantsViewModel Variants { get; set; }
        = new();

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        return Partial("List", await _variantService.GetVariants(Variants));
    }
    public IActionResult OnGetAdd()
    {
        return Partial("Add");
    }
    public async Task<IActionResult> OnPostAddAsync(AddVariantViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var variant = _mapper.Map<Entities.Variant>(model);
        variant.IsConfirmed = true;
        if (!variant.IsColor)
            variant.ColorCode = null;

        await _variantService.AddAsync(variant);
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, " تنوع مورد نظر با موفقیت اضافه شد"));
    }
    public async Task<IActionResult> OnGetEdit(long id)
    {
        var model = await _variantService.GetForEdit(id);
        if (model is null)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        }

        return Partial("Edit", model);
    }
    public async Task<IActionResult> OnPostEditAsync(EditVariantViewMode model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        var variant = await _variantService.FindByIdAsync(model.Id);
        if (variant is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        variant = _mapper.Map(model, variant);
        if (!variant.IsColor)
            variant.ColorCode = null;

        await _variantService.Update(variant);
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "تنوع مورد نظر با موفقیت ویرایش شد"));
    }
    public async Task<IActionResult> OnPostCheckForColorCode(string colorCode)
    {
        return Json(!await _variantService.IsExistsBy(nameof(Entities.Variant.ColorCode), colorCode));
    }
    public async Task<IActionResult> OnPostCheckForValue(string value)
    {
        return Json(!await _variantService.IsExistsBy(nameof(Entities.Variant.Value), value));
    }
    public async Task<IActionResult> OnPostCheckForColorCodeOnEdit(string colorCode,long id)
    {
        return Json(!await _variantService.IsExistsBy(nameof(Entities.Variant.ColorCode), colorCode, id));
    }
    public async Task<IActionResult> OnPostCheckForValueOnEdit(string value,long id)
    {
        return Json(!await _variantService.IsExistsBy(nameof(Entities.Variant.Value), value, id));
    }
    
   
}