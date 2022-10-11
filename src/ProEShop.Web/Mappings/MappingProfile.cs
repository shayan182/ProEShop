using ProEShop.Common.Helpers;
using ProEShop.Entities;
using ProEShop.Entities.Identity;
using ProEShop.ViewModels.Brands;
using ProEShop.ViewModels.Categories;
using ProEShop.ViewModels.CategoryFeatures;
using ProEShop.ViewModels.FeatureConstantValues;
using ProEShop.ViewModels.Product;
using ProEShop.ViewModels.Products;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Web.Mappings;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
       
        //if you wanna trim all of the strings (read and create)
        //CreateMap<string, string>()
        //    .ConvertUsing(str => str != null ? str.Trim() : null);

        CreateMap<User, CreateSellerViewModel>();
        CreateMap<CreateSellerViewModel, Seller>()
            .AddTransform<string>(str => str != null ? str.Trim() : null); // if you wanna trim strings (create)
        CreateMap<CreateSellerViewModel, User>()
            .AddTransform<string>(str => str != null ? str.Trim() : null) // if you wanna trim strings (create)
            .ForMember(x => x.BirthDate,
                opt => opt.Ignore());
        CreateMap<Seller, ShowSellerViewModel>()
            .ForMember(dest => dest.ProvinceAndCity,
                options =>
                    options.MapFrom(src => $"{src.Province.Title} - {src.City.Title}"))
            .ForMember(dest => dest.CreatedDateTime,
                options =>
                    options.MapFrom(src => src.CreatedDateTime.ToLongPersianDate()));
        CreateMap<Seller, SellerDetailsViewModel>()
            .ForMember(dest => dest.CreatedDateTime,
                options =>
                    options.MapFrom(src => src.CreatedDateTime.ToLongPersianDate()));
        CreateMap<Brand, ShowBrandViewModel>();
        CreateMap<AddBrandViewModel, Brand>();
        CreateMap<Brand, EditBrandViewMode>().ReverseMap()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        CreateMap<AddCategoryViewModel, Entities.Category>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        CreateMap<Entities.Category, EditCategoryViewModel>()
            .ForMember(x => x.Picture, // in the both class we have picture ,so mapper map these properties 
                opt => opt.Ignore()) 
            .ForMember(dest => dest.SelectedPicture,
                options =>
                    options.MapFrom(src => src.Picture));

        CreateMap<EditCategoryViewModel, Entities.Category>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        CreateMap<AddBrandBySellerViewModel, Entities.Brand>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);
        CreateMap<Brand, BrandDetailsViewModel>();
        this.CreateMap<Entities.CategoryFeature, CategoryFeatureForCreateProductViewModel>();
        this.CreateMap<FeatureConstantValue, ShowFeatureConstantValueViewModel>();
        CreateMap<AddFeatureConstantValueViewModel, Entities.FeatureConstantValue>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);
        this.CreateMap<FeatureConstantValue, ShowCategoryFeatureConstantValueViewModel>();

        CreateMap<FeatureConstantValue, EditFeatureConstantValueViewModel>();
        CreateMap<EditFeatureConstantValueViewModel, Entities.FeatureConstantValue>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        CreateMap<AddProductViewModel, Entities.Product>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);
        CreateMap<Entities.FeatureConstantValue, FeatureConstantValueForCreateProductViewModel>();
        CreateMap<Entities.Product, ShowProductViewModel>();

        this.CreateMap<Entities.Product, ShowProductViewModel>()
            .ForMember(dest => dest.MainPicture,
                options =>
                    options.MapFrom(src => src.ProductMedia.First().FileName));

        CreateMap<Entities.Product, ProductDetailsViewModel>()
            .ForMember(dest => dest.MainPicture,
                options =>
                    options.MapFrom(src => src.ProductMedia.First().FileName));
    }
}
