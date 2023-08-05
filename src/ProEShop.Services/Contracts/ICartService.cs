using ProEShop.Entities;
using ProEShop.ViewModels.Products;

namespace ProEShop.Services.Contracts;

public interface ICartService: ICustomGenericService<Cart>
{
    Task<List<ProductVariantInCartForProductInfoViewModel>> GetProductVariantsInCart(List<long> productVariantIds, long userId);

}