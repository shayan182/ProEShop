using ProEShop.Entities;

namespace ProEShop.Services.Contracts;

public interface ICategoryVariantService : ICustomGenericService<CategoryVariant>
{
    Task<List<long>> GetCategoryVariants(long categoryId);
}