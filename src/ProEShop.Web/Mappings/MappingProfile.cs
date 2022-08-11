using ProEShop.Common.Helpers;
using ProEShop.Entities;
using ProEShop.Entities.Identity;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Web.Mappings;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
       
        //if you wanna trim all of the strings (read and create)
        //this.CreateMap<string, string>()
        //    .ConvertUsing(str => str != null ? str.Trim() : null);

        this.CreateMap<User, CreateSellerViewModel>();
        this.CreateMap<CreateSellerViewModel, Seller>()
            .AddTransform<string>(str => str != null ? str.Trim() : null); // if you wanna trim strings (create)
        this.CreateMap<CreateSellerViewModel, User>()
            .AddTransform<string>(str => str != null ? str.Trim() : null) // if you wanna trim strings (create)
            .ForMember(x => x.BirthDate,
                opt => opt.Ignore());
        this.CreateMap<Seller, ShowSellerViewModel>()
            .ForMember(dest => dest.ProvinceAndCity,
                options =>
                    options.MapFrom(src => $"{src.Province.Title} - {src.City.Title}"))
            .ForMember(dest => dest.CreatedDateTime,
                options =>
                    options.MapFrom(src => src.CreatedDateTime.ToLongPersianDate()));
        this.CreateMap<Seller, SellerDetailsViewModel>()
            .ForMember(dest => dest.CreatedDateTime,
                options =>
                    options.MapFrom(src => src.CreatedDateTime.ToLongPersianDate())); ;

    }
}
