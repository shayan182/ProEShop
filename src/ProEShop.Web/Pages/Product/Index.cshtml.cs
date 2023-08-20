using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Products;

namespace ProEShop.Web.Pages.Product;

public class IndexModel : PageBase
{
    #region Constractor

    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IProductVariantService _productVariantService;
    private readonly IUserProductFavoriteService _userProductFavoriteService;
    private readonly IViewRendererService _viewRendererService;
    private readonly IUnitOfWork _uow;
    #endregion

    public IndexModel(IProductService productService, IUserProductFavoriteService userProductFavoriteService, IUnitOfWork uow, IProductVariantService productVariantService, ICartService cartService, IViewRendererService viewRendererService)
    {
        _productService = productService;
        _userProductFavoriteService = userProductFavoriteService;
        _uow = uow;
        _productVariantService = productVariantService;
        _cartService = cartService;
        _viewRendererService = viewRendererService;
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
        var productVariantsIds = ProductInfo.ProductVariants.Select(x => x.Id).ToList();
        var userId = User.Identity.GetLoggedInUserId();
        ProductInfo.ProductVariantsInCart = await _cartService.GetProductVariantsInCart(productVariantsIds, userId); return Page();
    }
    public async Task<IActionResult> OnPostAddOrRemoveFavorite(long productId, bool addFavorite)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new JsonResultOperation(false));
        }

        if (!await _productService.IsExistsBy(nameof(Entities.Product.Id), productId))
        {
            return Json(new JsonResultOperation(false));
        }

        var userId = User.Identity.GetLoggedInUserId();

        var userProductFavorite = await _userProductFavoriteService.FindAsync(userId, productId);
        if (userProductFavorite is null && addFavorite)
        {
            await _userProductFavoriteService.AddAsync(new Entities.UserProductFavorite
            {
                ProductId = productId,
                UserId = userId
            });
        }
        else if (userProductFavorite != null && !addFavorite)
        {
            _userProductFavoriteService.Remove(userProductFavorite);
        }

        await _uow.SaveChangesAsync();

        return Json(new JsonResultOperation(true, string.Empty));
    }

    public async Task<IActionResult> OnPostAddProductVariantToCart(long productVariantId , bool isIncrease)
    {
        var productVariant = await _productVariantService.FindByIdAsync(productVariantId);
        if (productVariant is null)
            return Json(new JsonResultOperation(false));

        var userId = User.Identity.GetLoggedInUserId();
        var cart = await _cartService.FindAsync(userId,productVariantId);
        if (cart is null)
        {
            var cartToAdd = new Entities.Cart()
            {
                ProductVariantId = productVariantId,
                UserId = userId,
                Count = 1
            };
            await _cartService.AddAsync(cartToAdd);

        }
        else
        {
            if (isIncrease)
            {
                cart.Count++;

                // check max value
                if (cart.Count > productVariant.MaxCountInCart)
                    cart.Count = productVariant.MaxCountInCart;

                // check stock
                if (cart.Count > productVariant.Count)
                    cart.Count = (short)productVariant.Count;
            }
            else
            {
                cart.Count--;
                if(cart.Count == 0)
                    _cartService.Remove(cart);
            }
        }

        await _uow.SaveChangesAsync();
        var isCartFull =  productVariant.MaxCountInCart == (cart?.Count ?? 1) // check max value
           || (cart?.Count ?? 1) == productVariant.Count;
        var carts = await _cartService.GetCartsForDropDown(userId);
        return Json(new JsonResultOperation(true,string.Empty)
        {
            Data = new
            {
                ProductVariantId = productVariantId,
                Count = cart ?.Count ??  1,
                IsCartFull = isCartFull,
                CartsDetails = await _viewRendererService.RenderViewToStringAsync("~/Pages/Shared/_CartPartial.cshtml",carts)
            }
        });
    }
}