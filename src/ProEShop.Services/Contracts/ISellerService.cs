using ProEShop.Entities;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Services.Contracts;
public interface ISellerService : IGenericService<Seller>
{
    Task<int> GetSellerCodeForCreateSeller();

    Task<ShowSellersViewModel> GetSellers(ShowSellersViewModel model);
    Task<SellerDetailsViewModel?> GetSellerDetails(long id);
    Task<Seller?> GetSellerToRemoveInManagingSeller(long id);
    Task<long> GetSellerIdAsync(long userId);
    /// <summary>
    /// Get Seller Id (if your user is already logged in)
    /// </summary>
    /// <returns></returns>
    Task<long> GetSellerIdAsync();
    Task<List<string?>> GetShopNamesForAutocomplete(string input);
}
