using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Carts;

namespace ProEShop.Web.Pages.Cart;

public class IndexModel : PageBase
{
    #region constructor

    private readonly ICartService _cartService;
    private readonly IProductVariantService _productVariantService;
    private readonly IUnitOfWork _uow;
    private readonly IViewRendererService _viewRendererService;

    public IndexModel(ICartService cartService, IViewRendererService viewRendererService, IUnitOfWork uow, IProductVariantService productVariantService)
    {
        _cartService = cartService;
        _viewRendererService = viewRendererService;
        _uow = uow;
        _productVariantService = productVariantService;
    }
    #endregion

    public List<ShowCartInCartPageViewModel> CartItems { get; set; }
    public async Task OnGet()
    {
        var now = DateTime.Now;
        var userId = User.Identity.GetLoggedInUserId();
        CartItems = await _cartService.GetCartsForCartPage(userId);

        //Custome
        //TODO IsDiscountActive in Cart page 
        foreach (var x in CartItems)
        {
            if (x.ProductVariantOffPercentage != null)
            {
                x.IsDiscountActive = x.ProductVariantStartDateTime <= now && x.ProductVariantEndDateTime >= now;
            }
        }
           
    }
    public async Task<IActionResult> OnPostAddProductVariantToCart(long productVariantId, bool isIncrease)
    {
        var productVariant = await _productVariantService.FindByIdAsync(productVariantId);
        if (productVariant is null)
            return Json(new JsonResultOperation(false));

        var userId = User.Identity.GetLoggedInUserId();
        var cart = await _cartService.FindAsync(userId, productVariantId);
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
                if (cart.Count == 0)
                    _cartService.Remove(cart);
            }
        }

        await _uow.SaveChangesAsync();
        var carts = await _cartService.GetCartsForCartPage(userId);

        var cartBody = string.Empty;

        if (carts.Count == 0)
        {
            cartBody = await _viewRendererService.RenderViewToStringAsync("~/Pages/Cart/_EmptyCartPartial.cshtml");
        }
        else
        {
            cartBody = await _viewRendererService.RenderViewToStringAsync("~/Pages/Cart/_CartBodyPartial.cshtml", carts);
        }

        return Json(new JsonResultOperation(true, string.Empty)
        {
            Data = new
            {
                CartBody = cartBody
            }
        });
    }
    public async Task<IActionResult> OnPostRemoveAllItemsInCart()
    {
        var userId = User.Identity.GetLoggedInUserId();
        var allItemsInCart = await _cartService.GetAllCartItems(userId);
        _cartService.RemoveRange(allItemsInCart);
        await _uow.SaveChangesAsync();
        return Json(new JsonResultOperation(true, string.Empty)
        {
            Data = new
            {
                CartBody = await _viewRendererService.RenderViewToStringAsync("~/Pages/Cart/_EmptyCartPartial.cshtml")
            }
        });
    }
}