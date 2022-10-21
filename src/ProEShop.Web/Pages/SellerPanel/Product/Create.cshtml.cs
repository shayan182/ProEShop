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
    private readonly ICategoryBrandService _categoryBrandService;

    public CreateModel(
        ICategoryService categoryService,
        IMapper mapper, IBrandService brandService, IUnitOfWork uow, IUploadFileService uploadFileService, ISellerService sellerService, ICategoryFeatureService categoryFeatureService, IFeatureConstantValueService featureConstantValueService, IViewRendererService viewRendererService, IHtmlSanitizer htmlSanitizer, IProductService productService, ICategoryBrandService categoryBrandService)
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
        _categoryBrandService = categoryBrandService;
    }

    #endregion

    public AddProductViewModel Product { get; set; }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPost(AddProductViewModel product)
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
        var CategoriesToAdd = await _categoryService.GetCategoryParentIds(product.MainCategoryId);
        if (!CategoriesToAdd.IsSuccessful)
            return Json(new JsonResultOperation(false));

        if (!await _categoryBrandService.CheckCategoryBrand(product.MainCategoryId, product.BrandId))
            return Json(new JsonResultOperation(false));

        var productToAdd = _mapper.Map<Entities.Product>(product);


        productToAdd.Slug = product.PersianTitle.ToUrlSlug();
        productToAdd.SellerId = await _sellerService.GetSelerId(User.Identity.GetLoggedInUserId());
        productToAdd.ShortDescription = _htmlSanitizer.Sanitize(product.ShortDescription);
        productToAdd.SpecialtyCheck = _htmlSanitizer.Sanitize(product.SpecialtyCheck);
        productToAdd.ProductCode = await _productService.GetProductCodeForCreateProduct();

        if (!await _categoryService.CanAddFakeProduct(product.MainCategoryId))
            productToAdd.IsFake = false;

        foreach (var categoryId in CategoriesToAdd.categoryIds)
        {
            productToAdd.ProductCategories.Add(new ProductCategory()
            {
                CategoryId = categoryId
            });
        }

        foreach (var picture in product.Pictures)
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

        foreach (var video in product.Videos)
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

        if (await _featureConstantValueService.CheckNonConstantValue(product.MainCategoryId, featureIds))
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
        if (!await _categoryFeatureService.CheckCategoryFeatureCount(product.MainCategoryId, featureIds))
        {
            return Json(new JsonResultOperation(false));
        }

        if (!await _featureConstantValueService.CheckConstantValue(product.MainCategoryId, featureConstantValueIds))
        {
            return Json(new JsonResultOperation(false));
        }

        var featureConstantValues =
            await _featureConstantValueService.GetFeatureConstantValuesForCreateProduct(product.MainCategoryId);
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
                        if (featureConstantValues
                            .Where(x => x.FeatureId == featureId)
                            .Any(x => x.Value == trimmedValue))
                        {
                            valueToAdd.Append(trimmedValue + "|||");
                        }
                    }
                    if (productToAdd.ProductFeatures.All(x => x.FeatureId != featureId))
                    {
                        if (valueToAdd.ToString().Length > 0)
                        {
                            // both of these code is same 
                            //var a = valueToAdd.ToString()[..(valueToAdd.Length - 3)];
                            //var b = valueToAdd.ToString().Substring(0, valueToAdd.Length - 3);
                            productToAdd.ProductFeatures.Add(new()
                            {
                                FeatureId = featureId,
                                Value = valueToAdd.ToString()[..(valueToAdd.Length - 3)]
                            });
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
        try
        {
            await _uow.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var productPictures = productToAdd.ProductMedia
            .Where(x => !x.IsVideo)
            .ToList();
        for (var counter = 0; counter < productPictures.Count; counter++)
        {
            var currentPicture = product.Pictures[counter];
            if (currentPicture.IsFileUploaded())
            {
                await _uploadFileService.SaveFile(currentPicture, productPictures[counter].FileName, null,
                    "images", "products");
            }
        }

        var productVideos = productToAdd.ProductMedia
            .Where(x => x.IsVideo)
            .ToList();
        for (var counter = 0; counter < productVideos.Count; counter++)
        {
            var currentVideo = product.Videos[counter];
            if (currentVideo.IsFileUploaded())
            {
                await _uploadFileService.SaveFile(currentVideo, productVideos[counter].FileName, null,
                    "videos", "products");
            }
        }
        return Json(new JsonResultOperation(true, "محصول مورد نظر با موفقیت ایجاد شد.")
        {
            Data = Url.Page(nameof(Successful))
        });
    }

    public void Successful()
    {

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
            Brands = await _brandService.GetBrandsByCategoryId(categoryId),
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

    public async Task<IActionResult> OnGetGetCommissionPercentage(long brandId , long categoryId)
    {
        if (brandId < 1 || categoryId < 1)
        {
            return Json(new JsonResultOperation(false));
        }

        var commissionPercentageResult = await _categoryBrandService.GetCommissionPercentage(categoryId,brandId);
        if (!commissionPercentageResult.IsSuccessfull)
        {
            return Json(new JsonResultOperation(false));
        }

        return Json(new JsonResultOperation(true,string.Empty)
        {
            Data = commissionPercentageResult.CommissionPercentage
        });
    }
}