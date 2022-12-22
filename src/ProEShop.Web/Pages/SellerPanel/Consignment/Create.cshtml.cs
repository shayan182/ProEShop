using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Consignments;

namespace ProEShop.Web.Pages.SellerPanel.Consignment;

public class CreateModel : SellerPanelBase
{
    #region Constructor

    private readonly IProductVariantService _productVariantService;
    private readonly IViewRendererService _rendererViewService;

    public CreateModel(IProductVariantService productVariantService, IViewRendererService rendererViewService)
    {
        _productVariantService = productVariantService;
        _rendererViewService = rendererViewService;
    }

    #endregion

    [BindProperty]
    [Display(Name = "کد تنوع")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Range(1, int.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public int VariantCode { get; set; }
    public CreateConsignmentViewModel CreateConsignment { get; set; }
    public void OnGet()
    {
    }
    public IActionResult OnPost(CreateConsignmentViewModel createConsignment)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        return Json(new JsonResultOperation(true, string.Empty));
    }

    public async Task<IActionResult> OnPostGetConsignmentTr()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var productVariant = await _productVariantService.GetProductVariantForCreateConsignment(VariantCode);
        if (productVariant is null)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        }

        return Json(new JsonResultOperation(true,String.Empty)
        {
            Data = await _rendererViewService.RenderViewToStringAsync(
                "~/Pages/SellerPanel/Consignment/_ProductVariantTrPartial.cshtml"
                , productVariant)
        });
    }
}