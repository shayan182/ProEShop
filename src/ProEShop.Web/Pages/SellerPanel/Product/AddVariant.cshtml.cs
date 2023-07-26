using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Variants;

namespace ProEShop.Web.Pages.SellerPanel.Product;

public class AddVariantModel : PageBase
{
    #region Constructor

    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ISellerService _sellerService;
    private readonly IProductVariantService _productVariantService;
    private readonly IGuaranteeService _guaranteeService;
    private readonly IVariantService _variantService;
    private readonly IUnitOfWork _uow;

    public AddVariantModel(IProductService productService, IMapper mapper, ISellerService sellerService, IProductVariantService productVariantService, IUnitOfWork uow, IGuaranteeService guaranteeService, IVariantService variantService)
    {
        _productService = productService;
        _mapper = mapper;
        _sellerService = sellerService;
        _productVariantService = productVariantService;
        _uow = uow;
        _guaranteeService = guaranteeService;
        _variantService = variantService;
    }

    #endregion

    [BindProperty]
    public AddVariantForSellerPanelViewModel Variant { get; set; }

    public async Task<IActionResult> OnGet(long productId)
    {
        var productInfo = await _productService.GetProductInfoForAddVariant(productId);
        if (productInfo is null)
        {
            return RedirectToPage(PublicConstantStrings.Error404PageName);
        }

        Variant = productInfo;
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        var checkInputs =
            await _variantService.CheckProductAndVariantTypeForForAddVariant(Variant.ProductId, Variant.VariantId);

        if (!checkInputs.IsSuccessful)
        {
            return Json(new JsonResultOperation(false));
        }

        var productVariant = _mapper.Map<ProductVariant>(Variant);
        if (checkInputs.IsVariantNull)
        {
            productVariant.VariantId = null;
        }
        productVariant.VariantCode = await _productVariantService.GetVariantCodeForCreateProductVariant();
        //Get Seller Id For Entity
        var sellerId = await _sellerService.GetSellerIdAsync();
        productVariant.SellerId = sellerId;

        await _productVariantService.AddAsync(productVariant);
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "تنوع محصول با موفقیت اضافه شد")
        {
            Data = Url.Page("SuccessfulProductVariant")
        });
    }

    public async Task<IActionResult> OnGetGetGuarantees(string input)
    {
        var result = await _guaranteeService.SearchOnGuaranteesForSelect2(input);
        // we most send a object of many array 
        return Json(new
        {
            results = result
        });
    }

}