using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.Services.Services;
using ProEShop.ViewModels.Brands;
using ProEShop.ViewModels.Categories;

namespace ProEShop.Web.Pages.Admin.Brand;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly IUploadFileService _uploadFileService;

    #endregion

    public IndexModel(IBrandService brandService, IMapper mapper, IUnitOfWork uow, IUploadFileService uploadFileService)
    {
        _brandService = brandService;
        _mapper = mapper;
        _uow = uow;
        _uploadFileService = uploadFileService;
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
        brand.LogoPicture = model.LogoPicture.GenerateFileName();
        string brandRegistrationFileName = null;
        if (model.BrandRegistrationPicture.IsFileUploaded())
            brandRegistrationFileName = model.BrandRegistrationPicture.GenerateFileName();
        brand.BrandRegistrationPicture = brandRegistrationFileName;

        await _brandService.AddAsync(brand);
        await _uow.SaveChangesAsync();
        await _uploadFileService.SaveFile(model.LogoPicture, brand.LogoPicture, null, "images", "brands");
        await _uploadFileService.SaveFile(model.BrandRegistrationPicture, brandRegistrationFileName, null, "images", "brandregistrationpictures");
        return Json(new JsonResultOperation(true, "برند مورد نظر با موفقیت اضافه شد"));
    }
}