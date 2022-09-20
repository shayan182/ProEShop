using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Brands;
using ProEShop.ViewModels.CategoryFeatures;
using ProEShop.ViewModels.Product;
using System.Text;

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
    private readonly IHtmlSanitizer _htmlSanitizer;
    private readonly IProductService _productService;

    public CreateModel(
        ICategoryService categoryService,
        IMapper mapper, IBrandService brandService, IUnitOfWork uow, IUploadFileService uploadFileService, ISellerService sellerService, ICategoryFeatureService categoryFeatureService, IFeatureConstantValueService featureConstantValueService, IViewRendererService viewRendererService, IHtmlSanitizer htmlSanitizer, IProductService productService)
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
        _htmlSanitizer = htmlSanitizer;
        _productService = productService;
    }

    #endregion

    [BindProperty]
    public AddProductViewModel Product { get; set; }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPost()
    {
        const string NonConstantInputName = "ProductFeatureValue";
        const string ConstantInputName = "ProductFeatureConstantValue";
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var productToAdd = _mapper.Map<Entities.Product>(Product);
        productToAdd.SellerId = await _sellerService.GetSelerId(User.Identity.GetLoggedInUserId());
        productToAdd.ShortDescription = _htmlSanitizer.Sanitize(Product.ShortDescription);
        productToAdd.SpecialtyCheck = _htmlSanitizer.Sanitize(Product.SpecialtyCheck);

        var CategoriesToAdd = await _categoryService.GetCategoryParentIds(Product.CategoryId);
        if (!CategoriesToAdd.IsSuccessful)
        {
            return Json(new JsonResultOperation(false));
        }

        foreach (var categoryId in CategoriesToAdd.categoryIds)
        {
            productToAdd.ProductCategories.Add(new ProductCategory()
            {
                CategoryId = categoryId
            });
        }

        foreach (var picture in Product.Pictures)
        {
            if (picture.IsFileUploaded())
            {
                var filename = picture.GenerateFileName();
                productToAdd.ProductMedia.Add(new ProductMedia()
                {
                    FileName = filename,
                    IsVideo = false
                });
            }
        }

        foreach (var video in Product.Videos)
        {
            if (video.IsFileUploaded())
            {
                var filename = video.GenerateFileName();
                productToAdd.ProductMedia.Add(new ProductMedia()
                {
                    FileName = filename,
                    IsVideo = true
                });
            }
        }

        #region NonConstantFeature

        var featureIds = new List<long>();
        var productFeatureValueInputs = Request.Form
            .Where(x => x.Key.StartsWith(NonConstantInputName)).ToList();
        foreach (var item in productFeatureValueInputs)
        {
            if (long.TryParse(item.Key.Replace(NonConstantInputName, String.Empty), out var featureId))
            {
                featureIds.Add(featureId);
            }
            else
            {
                return Json(new JsonResultOperation(false));
            }
        }

        if (await _featureConstantValueService.CheckNonConstantValue(Product.CategoryId, featureIds))
        {
            return Json(new JsonResultOperation(false));
        }
        foreach (var item in productFeatureValueInputs)
        {

            if (long.TryParse(item.Key.Replace(NonConstantInputName, String.Empty), out var featureId))
            {
                var trimmedValue = item.Value.ToString().Trim();
                if (productToAdd.ProductFeatures.All(x => x.FeatureId != featureId))
                {
                    if (trimmedValue.Length > 0)
                    {
                        productToAdd.ProductFeatures.Add(new ProductFeature()
                        {
                            FeatureId = featureId,
                            Value = trimmedValue
                        });
                        featureIds.Add(featureId);
                    }
                }
            }
            else
            {
                return Json(new JsonResultOperation(false));
            }
        }
        #endregion

        #region ConstantFeature

        var featureConstantValueIds = new List<long>();
        var productFeatureConstantValueInputs = Request.Form
            .Where(x => x.Key.StartsWith(ConstantInputName)).ToList();
        foreach (var item in productFeatureConstantValueInputs)
        {
            var x = item.Key.Replace(ConstantInputName, String.Empty);
            if (long.TryParse(item.Key.Replace(ConstantInputName, String.Empty), out var featureId))
            {
                featureConstantValueIds.Add(featureId);
            }
            else
            {
                return Json(new JsonResultOperation(false));
            }
        }

        //ckeck for both of the lists(nonConstant and Constant)
        featureIds = featureIds.Concat(featureConstantValueIds).ToList();
        if (!await _categoryFeatureService.CheckCategoryFeatureCount(Product.CategoryId, featureIds))
        {
            return Json(new JsonResultOperation(false));
        }

        if (!await _featureConstantValueService.CheckConstantValue(Product.CategoryId, featureConstantValueIds))
        {
            return Json(new JsonResultOperation(false));
        }

        foreach (var item in productFeatureConstantValueInputs)
        {
            if (long.TryParse(item.Key.Replace(ConstantInputName, string.Empty), out var featureId))
            {
                if (item.Value.Count > 0)
                {
                    var valueToAdd = new StringBuilder();
                    foreach (var value in item.Value)
                    {
                        var trimmedValue = value.Trim();
                        if (trimmedValue.Length > 0)
                        {
                            valueToAdd.Append(trimmedValue + "|||");
                        }
                    }
                    if (productToAdd.ProductFeatures.All(x => x.FeatureId != featureId))
                    {
                        if (valueToAdd.ToString().Length > 0)
                        {
                            productToAdd.ProductFeatures.Add(new ProductFeature()
                            {
                                FeatureId = featureId,
                                Value = valueToAdd.ToString().Substring(0, valueToAdd.Length - 3)
                            });
                            featureConstantValueIds.Add(featureId);
                        }
                    }
                }
            }
            else
            {
                return Json(new JsonResultOperation(false));
            }
        }
        #endregion
        await _productService.AddAsync(productToAdd);
        await _uow.SaveChangesAsync();

        var productPictures = productToAdd.ProductMedia
            .Where(x => !x.IsVideo)
            .ToList();
        for (var counter = 0; counter < productPictures.Count; counter++)
        {
            var currentPicture = Product.Pictures[counter];
            if (currentPicture.IsFileUploaded())
            {
                await _uploadFileService.SaveFile(currentPicture, productPictures[counter].FileName, null,
                    "images", "product");
            }
        }

        var productVideos = productToAdd.ProductMedia
            .Where(x => x.IsVideo)
            .ToList();
        for (var counter = 0; counter < productVideos.Count; counter++)
        {
            var currentVideo = Product.Videos[counter];
            if (currentVideo.IsFileUploaded())
            {
                await _uploadFileService.SaveFile(currentVideo, productVideos[counter].FileName, null,
                    "videos", "product");
            }
        }
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