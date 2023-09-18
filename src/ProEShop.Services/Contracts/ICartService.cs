using ProEShop.Entities;
using ProEShop.ViewModels.Carts;
using ProEShop.ViewModels.Products;

namespace ProEShop.Services.Contracts;

public interface ICartService: ICustomGenericService<Cart>
{
    Task<List<ProductVariantInCartForProductInfoViewModel>> GetProductVariantsInCart(List<long> productVariantIds, long userId);
    Task<List<ShowCartInDropDownViewModel>> GetCartsForDropDown(long userId);
    Task<List<ShowCartInCartPageViewModel>> GetCartsForCartPage(long userId);
    Task<List<ShowCartInCheckoutPageViewModel>> GetCartsForCheckoutPage(long userId);
    Task<List<ShowCartInPaymentPageViewModel>> GetCartsForPaymentPage(long userId);
    Task<List<ShowCartForCreateOrderAndPayViewModel>> GetCartsForCreateOrderAndPay(long userId);
    Task<List<Entities.Cart>> GetAllCartItems(long userId);
}