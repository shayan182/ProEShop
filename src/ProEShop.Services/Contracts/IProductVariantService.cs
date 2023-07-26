using ProEShop.Entities;
using ProEShop.ViewModels.ProductVariants;

namespace ProEShop.Services.Contracts;

public interface IProductVariantService : IGenericService<ProductVariant>
{
    Task<List<ShowProductVariantViewModel>> GetProductVariants(long productId);
    Task<int> GetVariantCodeForCreateProductVariant();
    Task<ShowProductVariantInCreateConsignmentViewModel?> GetProductVariantForCreateConsignment(int variantCode);

    Task<List<GetProductVariantInCreateConsignmentViewModel>> GetProductVariantForCreateConsignment(
        List<int> variantCodes);
    Task<List<ProductVariant>> GetProductVariantsToAddCount(List<long> Ids);
    Task<EditProductVariantViewModel?> GetDataForEdit(long id);
    Task<AddEditDiscountViewModel?> GetDataForAddEditDiscount(long id);
    Task<ProductVariant?> GetForEdit(long id);
    Task<List<long>> GetAddedVariantsToProductVariants(List<long> variantIds,long categoryId);
}