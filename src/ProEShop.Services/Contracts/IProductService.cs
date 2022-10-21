using ProEShop.Entities;
using ProEShop.ViewModels.Products;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Services.Contracts;

public interface IProductService: IGenericService<Product>
{
    Task<ShowProductsViewModel> GetProducts(ShowProductsViewModel model);
    Task<ShowProductsInSellerPanelViewModel> GetProductsInSellerPanel(ShowProductsInSellerPanelViewModel model);
    Task<List<string?>> GetPersianTitlesForAutocomplete(string input);
    Task<ProductDetailsViewModel?> GetProductDetails(long productId);
    Task<Product?> GetProductToRemoveInManagingProducts(long id);

    /// <summary>
    /// گرفتن آخرین کد محصول به علاوه یک
    /// جهت استفاده در صفحه افزودن محصول
    /// </summary>
    /// <returns></returns>
    Task<int> GetProductCodeForCreateProduct();
}