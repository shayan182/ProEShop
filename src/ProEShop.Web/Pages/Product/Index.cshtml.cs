using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProEShop.Common.Constants;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Products;

namespace ProEShop.Web.Pages.Product;

public class IndexModel : PageModel
{
    #region Constractor

    private readonly IProductService _productService;

    #endregion

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public ShowProductInfoViewModel? ProductInfo { get; set; }
    public async Task<IActionResult> OnGet(int productCode, string slug)
    {
        ProductInfo = await _productService.GetProductInfo(productCode);
        if (ProductInfo is null)
        {
            return RedirectToPage(PublicConstantStrings.Error404PageName);
        }

        //SEO Tip
        if (slug != ProductInfo.Slug)
        {
            return RedirectToPage("Index", new
            {
                productCode,
                slug = ProductInfo.Slug
            });
        }
        return Page();
    }
}