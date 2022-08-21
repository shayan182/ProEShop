using ProEShop.Entities;

namespace ProEShop.Services.Contracts;

public interface ICategoryFeatureService : ICustomGenericService<CategoryFeature>
{
    Task<CategoryFeature?> GetCategoryFeature(long categoryId, long featureId);
}