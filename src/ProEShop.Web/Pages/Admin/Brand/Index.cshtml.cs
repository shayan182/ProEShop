using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Brands;

namespace ProEShop.Web.Pages.Admin.Brand;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly IUploadFileService _uploadFileService;
    private readonly IHtmlSanitizer _htmlSanitizer;

    #endregion

    public IndexModel(
        IBrandService brandService,
        IMapper mapper,
        IUnitOfWork uow,
        IUploadFileService uploadFileService,
        IHtmlSanitizer htmlSanitizer)
    {
        _brandService = brandService;
        _mapper = mapper;
        _uow = uow;
        _uploadFileService = uploadFileService;
        _htmlSanitizer = htmlSanitizer;
    }

    [BindProperty(SupportsGet = true)]
    public ShowBrandsViewModel Brands { get; set; }
        = new();

    public void OnGet()
    {
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

        return Partial("List", await _brandService.GetBrands(Brands));
    }

    public IActionResult OnGetAdd()
    {
        return Partial("Add");
    }

    public async Task<IActionResult> OnPostAddAsync(AddBrandViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var brand = _mapper.Map<Entities.Brand>(model);
        brand.Description = _htmlSanitizer.Sanitize(model.Description);
        brand.IsConfirmed = true;

        string brandLogoFileName = null;
        if (model.LogoPicture.IsFileUploaded())
            brandLogoFileName = model.LogoPicture.GenerateFileName();
        brand.LogoPicture = brandLogoFileName;

        string brandRegistrationFileName = null;
        if (model.BrandRegistrationPicture.IsFileUploaded())
            brandRegistrationFileName = model.BrandRegistrationPicture.GenerateFileName();
        brand.BrandRegistrationPicture = brandRegistrationFileName;

        var result = await _brandService.AddAsync(brand);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<AddBrandViewModel>()
            });
        }

        await _uow.SaveChangesAsync();
        await _uploadFileService.SaveFile(model.LogoPicture, brand.LogoPicture, null, "images", "brands");
        await _uploadFileService.SaveFile(model.BrandRegistrationPicture, brandRegistrationFileName, null, "images",
            "brandregistrationpictures");
        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت اضافه شد"));
    }

    public async Task<IActionResult> OnGetEdit(long id)
    {
        var model = await _brandService.GetForEdit(id);
        if (model is null)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        }

        return Partial("Edit", model);
    }

    public async Task<IActionResult> OnPostEditAsync(EditBrandViewMode model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var brandToUpdate = await _brandService.FindByIdAsync(model.Id);
        if (brandToUpdate is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));

        var oldLogoPictureFileName = brandToUpdate.LogoPicture;
        var oldBrandRegistrationFileName = brandToUpdate.BrandRegistrationPicture;

        brandToUpdate = _mapper.Map(model, brandToUpdate);

        string logoPictureFileName = null;
        if (model.NewLogoPicture.IsFileUploaded())
            logoPictureFileName = model.NewLogoPicture.GenerateFileName();
        brandToUpdate.LogoPicture = logoPictureFileName == null ? oldLogoPictureFileName : logoPictureFileName;

        string brandRegistrationFileName = null;
        if (model.NewBrandRegistrationPicture.IsFileUploaded())
            brandRegistrationFileName = model.NewBrandRegistrationPicture.GenerateFileName();
        brandToUpdate.BrandRegistrationPicture = brandRegistrationFileName == null
            ? oldBrandRegistrationFileName
            : brandRegistrationFileName;


        var result = await _brandService.Update(brandToUpdate);
        if (!result.Ok)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = result.Columns.SetDuplicateColumnsErrors<EditBrandViewMode>()
            });
        }

        await _uow.SaveChangesAsync();
        await _uploadFileService.SaveFile(model.NewLogoPicture, brandToUpdate.LogoPicture, oldLogoPictureFileName,
            "images", "brands");
        await _uploadFileService.SaveFile(model.NewBrandRegistrationPicture, brandToUpdate.BrandRegistrationPicture,
            oldBrandRegistrationFileName, "images", "brandregistrationpictures");
        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت ویرایش شد"));
    }

    public async Task<IActionResult> OnGetGetBrandDetailsAsync(long brandId)
    {
        var model = await _brandService.GetBrandDetails(brandId);
        if (model is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        return Partial("BrandDetails", model);
    }

    public async Task<IActionResult> OnPostRejectBrand(BrandDetailsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, "لطفا دلیل رد برند را وارد نمایید"));
        }

        var brand = await _brandService.GetInActiveBrand(model.Id);
        if (brand is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        _brandService.Remove(brand);
        await _uow.SaveChangesAsync();
        //todo: send reject reasons to seller Email
        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت حذف شد"));
    }
    public async Task<IActionResult> OnPostConfirmBrand(long id)
    {
        var brand = await _brandService.GetInActiveBrand(id);
        if (brand is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        brand.IsConfirmed = true;
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت شد"));
    }

    public async Task<IActionResult> OnPostCheckForTitleFa(string titleFa)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleFa), titleFa));
    }
    public async Task<IActionResult> OnPostCheckForTitleEn(string titleEn)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleEn), titleEn));
    }
    public async Task<IActionResult> OnPostCheckForTitleFaOnEdit(string titleFa, long id)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleFa), titleFa, id));
    }
    public async Task<IActionResult> OnPostCheckForTitleEnOnEdit(string titleEn, long id)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleEn), titleEn, id));
    }
}