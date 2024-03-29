﻿using ProEShop.Entities;
using ProEShop.ViewModels.Products;
using ProEShop.ViewModels.Sellers;
using ProEShop.ViewModels.Variants;

namespace ProEShop.Services.Contracts;

public interface IProductService: IGenericService<Product>
{
    Task<ShowProductsViewModel> GetProducts(ShowProductsViewModel model);
    Task<ShowProductsInSellerPanelViewModel> GetProductsInSellerPanel(ShowProductsInSellerPanelViewModel model);
    Task<ShowAllProductsInSellerPanelViewModel> GetAllProductsInSellerPanel(ShowAllProductsInSellerPanelViewModel model);
    Task<List<string?>> GetPersianTitlesForAutocomplete(string input);
    Task<ProductDetailsViewModel?> GetProductDetails(long productId);
    Task<Product?> GetProductToRemoveInManagingProducts(long id);

    /// <summary>
    /// گرفتن آخرین کد محصول به علاوه یک
    /// جهت استفاده در صفحه افزودن محصول
    /// </summary>
    /// <returns></returns>
    Task<int> GetProductCodeForCreateProduct();

    Task<List<string?>> GetPersianTitlesForAutocompleteInSellerPanel(string input);
    Task<AddVariantForSellerPanelViewModel?> GetProductInfoForAddVariant(long productId);
    Task<ShowProductInfoViewModel?> GetProductInfo(long productCode);
    Task<(int productCode, string slug)> FindByShortLink(string productShortLink);
    Task<List<Product>> GetProductsForChangeStatus(List<long> ids);
}