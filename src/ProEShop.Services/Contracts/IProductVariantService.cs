using ProEShop.Entities;
using ProEShop.ViewModels.ProductVariants;

namespace ProEShop.Services.Contracts;

public interface IProductVariantService: IGenericService<ProductVariant>
{
    Task<List<ShowProductVariantViewModel>> GetProductVariants(long productId);
    Task<int> GetVariantCodeForCreateProductVariant();
    Task<ShowProductVariantInCreateConsignmentViewModel?> GetProductVariantForCreateConsignment(int variantCode);
}