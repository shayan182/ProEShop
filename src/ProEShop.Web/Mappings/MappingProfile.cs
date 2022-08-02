using ProEShop.Entities;
using ProEShop.Entities.Identity;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Web.Mappings;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        this.CreateMap<string, string>()
            .ConvertUsing(str => str == null ? null : str.Trim());
        this.CreateMap<User, CreateSellerViewModel>();
        this.CreateMap<CreateSellerViewModel, Seller>();
        this.CreateMap<CreateSellerViewModel, User>()
            .ForMember(x => x.BirthDate,
                opt => opt.Ignore()); ;
    }
}
