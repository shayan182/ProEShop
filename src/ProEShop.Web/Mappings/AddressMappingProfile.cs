using AutoMapper;
using ProEShop.ViewModels.Carts;

namespace ProEShop.Web.Mappings;

public class AddressMappingProfile : Profile
{
    public AddressMappingProfile()
    {
        this.CreateMap<Entities.Address, AddressInCheckoutPageViewModel>();
    }
}
