using ProEShop.Entities;
using ProEShop.ViewModels.Brands;

namespace ProEShop.Services.Contracts;

public interface IBrandService: IGenericService<Brand>
{
    Task<ShowBrandsViewModel> GetBrands(ShowBrandsViewModel model);

    Task<EditBrandViewMode?> GetForEdit(long id);
    Task<List<string>> AutoCompleteSearch(string input);
    Task<Dictionary<long, string>> GetBrandsByFullTitle(List<string> brandTitles);
    Task<BrandDetailsViewModel?> GetBrandDetails(long brandId);
    Task<Dictionary<long, string>> GetBrandsByCategoryId(long categoryId);
    Task<Brand?> GetInActiveBrand(long brandId);

}