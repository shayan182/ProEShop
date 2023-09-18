using ProEShop.Entities;
using ProEShop.ViewModels.Carts;

namespace ProEShop.Services.Contracts;

public interface IAddressService: IGenericService<Address>
{
    Task<AddressInCheckoutPageViewModel> GetAddressForCheckoutPage(long userId);
    Task<(bool HasUserAddress ,long AddressId)> GetAddressForCreateOrderAndPay(long userId);
}