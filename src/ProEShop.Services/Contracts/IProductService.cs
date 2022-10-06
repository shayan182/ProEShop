using ProEShop.Entities;
using ProEShop.ViewModels.Products;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Services.Contracts;

public interface IProductService: IGenericService<Product>
{
    Task<ShowProductsViewModel> GetProducts(ShowProductsViewModel model);
    Task<List<string?>> GetPersianTitlesForAutocomplete(string input);
}