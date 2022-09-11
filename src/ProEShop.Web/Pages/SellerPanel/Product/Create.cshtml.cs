using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.Services.Services;
using ProEShop.ViewModels.Brands;
using ProEShop.ViewModels.CategoryFeatures;
using ProEShop.ViewModels.Product;

namespace ProEShop.Web.Pages.SellerPanel.Product;

public class CreateModel : SellerPanelBase
{
    #region Ctor

    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly IBrandService _brandService;
    private readonly IUnitOfWork _uow;
    private readonly IUploadFileService _uploadFileService;
    private readonly ISellerService _sellerService;
    private readonly ICategoryFeatureService _categoryFeatureService;
    private readonly IViewRendererService _viewRendererService;
    private readonly IFeatureConstantValueService _featureConstantValueService;

    public CreateModel(
        ICategoryService categoryService,
        IMapper mapper, IBrandService brandService, IUnitOfWork uow, IUploadFileService uploadFileService, ISellerService sellerService, ICategoryFeatureService categoryFeatureService, IFeatureConstantValueService featureConstantValueService, IViewRendererService viewRendererService)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _brandService = brandService;
        _uow = uow;
        _uploadFileService = uploadFileService;
        _sellerService = sellerService;
        _categoryFeatureService = categoryFeatureService;
        _featureConstantValueService = featureConstantValueService;
        _viewRendererService = viewRendererService;
    }

    #endregion

    public AddProductViewModel Product { get; set; }
    public void OnGet()
    {
    }
    public IActionResult OnPost(AddProductViewModel model)
    {
        return Json(new JsonResultOperation(true, "message")
        {
            Data = "nothing"
        });
    }
    public async Task<IActionResult> OnGetGetCategories(long[] selectedCategoriesIds)
    {
        var result = await _categoryService.GetCategoriesForCreateProduct(selectedCategoriesIds);
        return Partial("_SelectProductCategoryPartial", result);
    }
    public async Task<IActionResult> OnGetGetCategoryInfo(long categoryId)
    {
        if (categoryId < 1)
            return Json(new JsonResultOperation(false));

        var categoryFeatureModel = new ProductFeaturesForCreateProductViewModel
        {
            Features = await _categoryFeatureService.GetCategoryFeatures(categoryId),
            FeaturesConstantValues = await _featureConstantValueService.GetFeatureConstantValuesByCategoryId(categoryId)
        };

        var model = new
        {
            Brands = await _categoryService.GetBrandsByCategoryId(categoryId),
            CanAddFakeProduct = await _categoryService.CanAddFakeProduct(categoryId),
            CategoryFeatures = await _viewRendererService.RenderViewToStringAsync(
                "~/Pages/SellerPanel/Product/_ShowCategoryFeaturesPartial.cshtml", categoryFeatureModel)
        };
        return Json(new JsonResultOperation(true, String.Empty)
        {
            Data = model
        });
    }
    public IActionResult OnGetRequestForAddBrand(long categoryId)
    {
        return Partial("_RequestForAddBrandPartial");
    }
    public async Task<IActionResult> OnPostRequestForAddBrand(AddBrandBySellerViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        var brand = _mapper.Map<Entities.Brand>(model);

        //add brand category
        brand.CategoryBrands.Add(new CategoryBrand()
        {
            CategoryId = model.CategoryId
        });

        var userId = User.Identity?.GetUserId();
        brand.SellerId = await _sellerService.GetSelerId(userId.Value);
        brand.LogoPicture = model.LogoPicture.GenerateFileName();
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
        await _uploadFileService.SaveFile(model.BrandRegistrationPicture, brandRegistrationFileName, null, "images", "brandregistrationpictures");
        return Json(new JsonResultOperation(true, "برند ثبت شد و پس از تایید کارشناسان قابل دسترسی است، مراتب از طریق ایمیل به شما اطلاع رسانی خواهد شد"));
    }

    public IActionResult OnPostUploadSpecialtyCheckImages([IsImage] IFormFile file)
    {
        if (ModelState.IsValid && file.IsFileUploaded())
        {
            var imageFileName = file.GenerateFileName();
            _uploadFileService.SaveFile(file, imageFileName, null, "images", "products", "specialty-check-images");
            return Json(new
            {
                location = $"/images/products/specialty-check-images/{imageFileName}"
            });
        }
        return Json(false);
    }
    public IActionResult OnPostUploadShortDescriptionImages([IsImage] IFormFile file)
    {
        if (ModelState.IsValid && file.IsFileUploaded())
        {
            var imageFileName = file.GenerateFileName();
            _uploadFileService.SaveFile(file, imageFileName, null, "images", "products", "short-description-images");
            return Json(new
            {
                location = $"/images/products/short-description-images/{imageFileName}"
            });
        }
        return Json(false);
    }
    public async Task<IActionResult> OnGetShowCategoryFeatures(long categoryId)
    {
        var model = new
        {
            feature = await _categoryFeatureService.GetCategoryFeatures(categoryId),
            featureConstantValue = await _featureConstantValueService.GetFeatureConstantValuesByCategoryId(categoryId)
        };
        return Partial("_ShowCategoryFeaturesPartial", model);
    }
    public async Task<IActionResult> OnPostCheckForTitleFa(string titleFa)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleFa), titleFa));
    }
    public async Task<IActionResult> OnPostCheckForTitleEn(string titleEn)
    {
        return Json(!await _brandService.IsExistsBy(nameof(Entities.Brand.TitleEn), titleEn));
    }
}