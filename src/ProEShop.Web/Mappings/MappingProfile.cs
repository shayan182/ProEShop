using ProEShop.Entities;
using ProEShop.Entities.Identity;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Web.Mappings;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        this.CreateMap<User, CreateSellerViewModel>();
        this.CreateMap<CreateSellerViewModel, Seller>();
    }
}
