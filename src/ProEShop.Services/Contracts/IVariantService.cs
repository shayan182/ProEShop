using ProEShop.Entities;
using ProEShop.ViewModels.Brands;
using ProEShop.ViewModels.CategoryVariants;
using ProEShop.ViewModels.Variants;

namespace ProEShop.Services.Contracts;

public interface IVariantService : IGenericService<Variant>
{
    Task<ShowVariantsViewModel> GetVariants(ShowVariantsViewModel model);
    Task<EditVariantViewMode?> GetForEdit(long id);
    Task<(bool IsSuccessful, bool IsVariantNull)> CheckProductAndVariantTypeForForAddVariant(long productId, long variantId);
    Task<List<ShowVariantInEditCategoryVariantViewModel>> GetVariantsForEditCategoryVariants(bool isColor);
    Task<bool> CheckVariantsCountAndConfirmStatusForEditCategoryVariants(List<long> variantsIds, bool isColor);

}