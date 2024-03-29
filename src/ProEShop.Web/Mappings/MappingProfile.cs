﻿using System.Globalization;
using AutoMapper;
using ProEShop.Common.Helpers;
using ProEShop.Entities;
using ProEShop.Entities.Identity;
using ProEShop.ViewModels.Brands;
using ProEShop.ViewModels.Categories;
using ProEShop.ViewModels.CategoryFeatures;
using ProEShop.ViewModels.Consignments;
using ProEShop.ViewModels.FeatureConstantValues;
using ProEShop.ViewModels.Guarantees;
using ProEShop.ViewModels.Product;
using ProEShop.ViewModels.Products;
using ProEShop.ViewModels.ProductShortLinks;
using ProEShop.ViewModels.ProductStocks;
using ProEShop.ViewModels.ProductVariants;
using ProEShop.ViewModels.Sellers;
using ProEShop.ViewModels.Variants;
using ProEShop.Web.Pages.Inventory.ProductStock;
using EditCategoryViewModel = ProEShop.ViewModels.Categories.EditCategoryViewModel;

namespace ProEShop.Web.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region parameters

        long userId = 0;
        long consignmentId = 0;
        DateTime now = default;


        #endregion

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
                    options.MapFrom(src => src.Picture))
            .ForMember(dest => dest.CanVariantTypeChange,
                options =>
                    options.MapFrom(src => src.CategoryVariants.Any() ? false : (!src.HasVariant)));

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
        this.CreateMap<Entities.Product, ShowProductInSellerPanelViewModel>()
            .ForMember(dest => dest.MainPicture,
                options =>
                    options.MapFrom(src => src.ProductMedia.First().FileName));

        this.CreateMap<Entities.Product, ShowAllProductInSellerPanelViewModel>()
            .ForMember(dest => dest.MainPicture,
                options =>
                    options.MapFrom(src => src.ProductMedia.First().FileName));
        CreateMap<Entities.Product, ProductDetailsViewModel>();
        this.CreateMap<Entities.ProductFeature, ProductFeatureForProductDetailsViewModel>();
        this.CreateMap<Entities.Variant, ShowVariantViewModel>();
        this.CreateMap<AddVariantViewModel, Entities.Variant>()
            .AddTransform<string>(str => str != null ? str.Trim() : null);

        this.CreateMap<EditVariantViewMode, Entities.Variant>().ReverseMap()
            .AddTransform<string>(str => str != null ? str.Trim() : null);
        this.CreateMap<Entities.Guarantee, ShowGuaranteeViewModel>();
        this.CreateMap<Entities.Guarantee, AddGuaranteeViewModel>().ReverseMap();
        this.CreateMap<Entities.Guarantee, EditGuaranteeViewMode>();
        this.CreateMap<Entities.Product, AddVariantForSellerPanelViewModel>()
            .ForMember(dest => dest.ProductId,
                options =>
                    options.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductTitle,
                options =>
                    options.MapFrom(src => src.PersianTitle))
        //.ForMember(x => x.CommissionPercentage,
        //    opt => opt.Ignore());

        .ForMember(dest => dest.CommissionPercentage,
            options =>
                options.MapFrom(
                    src => src.Category.CategoryBrands
                        .Select(x => new
                        {
                            x.BrandId,
                            x.CommissionPercentage
                        })
                        .Single(x => x.BrandId == src.BrandId)
                        .CommissionPercentage
                ))
            .ForMember(dest => dest.MainPicture,
                options =>
                    options.MapFrom(src => src.ProductMedia.First().FileName))
            .ForMember(dest => dest.Variants,
                options =>
                    options.MapFrom(src =>
                        src.Category.CategoryVariants.Where(x => x.Variant.IsConfirmed)
                        ));

        this.CreateMap<Entities.CategoryVariant, ShowCategoryVariantInAddVariantViewModel>();
        this.CreateMap<AddVariantForSellerPanelViewModel, ProductVariant>();
        this.CreateMap<Entities.ProductVariant, ShowProductVariantViewModel>()
            .ForMember(dest => dest.StartDateTime,
                options =>
                    options.MapFrom(src =>
                        src.StartDateTime != null
                            ? src.StartDateTime.Value.ToLongPersianDate()
                            : null))
            .ForMember(dest => dest.EndDateTime,
                options =>
                    options.MapFrom(src =>
                        src.EndDateTime != null
                            ? src.EndDateTime.Value.ToLongPersianDate()
                            : null));
        this.CreateMap<ProductVariant, ShowProductVariantInCreateConsignmentViewModel>();
        this.CreateMap<ProductVariant, GetProductVariantInCreateConsignmentViewModel>();
        this.CreateMap<Entities.Consignment, ShowConsignmentViewModel>()
            .ForMember(dest => dest.DeliveryDate,
                options =>
                    options.MapFrom(src => src.DeliveryDate.ToLongPersianDate()));
        this.CreateMap<Entities.Consignment, ShowConsignmentDetailsViewModel>()
            .ForMember(dest => dest.DeliveryDate,
                options =>
                    options.MapFrom(src => src.DeliveryDate.ToLongPersianDate()))
            .ForMember(dest => dest.Items,
            options =>
                options.MapFrom(src => src.ConsignmentItems.Where(x => x.ConsignmentId == consignmentId)));
        this.CreateMap<ConsignmentItem, ShowConsignmentItemViewModel>();
        this.CreateMap<AddProductStockByConsignmentViewModel, Entities.ProductStock>();

        this.CreateMap<Entities.Product, ShowProductInfoViewModel>()
            .ForMember(dest => dest.Score,
                options =>
                    options.MapFrom(src =>
                        src.ProductComments!.Any() ?
                            src.ProductComments!.Average(pc => pc.Score)
                           : 0))
            .ForMember(dest => dest.ProductCommentsCount,
                options =>
                    options.MapFrom(src =>
                        src.ProductComments!.LongCount(pc => pc.CommentTitle != null)))
            .ForMember(dest => dest.SuggestCount,
                options =>
                    options.MapFrom(src =>
                        src.ProductComments!
                            .Where(x => x.IsBuyer)
                            .LongCount(pc => pc.Suggest == true)))
            .ForMember(dest => dest.BuyerCount,
                options =>
                    options.MapFrom(src =>
                        src.ProductComments!
                            .LongCount(x => x.IsBuyer)))
            //.ForMember(dest => dest.ProductVariants,
            //    options =>
            //        options.MapFrom(src =>
            //            src.ProductVariants!
            //                .Where(x => x.Count > 0)))
            .ForMember(dest => dest.ProductVariants,
                options =>
                    options.MapFrom(src =>
                        src.ProductVariants.Where(x => x.Count > 0)
                    ))
            .ForMember(dest => dest.IsFavorite,
                options =>
                    options.MapFrom(src =>
                        userId != 0 && src.UserProductFavorites.Any(x => x.UserId == userId)))
            .ForMember(dest => dest.IsVariantTypeNull,
                options =>
                    options.MapFrom(src => src.Category.IsVariantColor == null
                       ));

        this.CreateMap<Entities.ProductCategory, ProductCategoryForProductInfoViewModel>();
        this.CreateMap<Entities.ProductFeature, ProductFeatureForProductInfoViewModel>();
        this.CreateMap<Entities.ProductVariant, ProductVariantForProductInfoViewModel>()
            .ForMember(dest => dest.EndDateTime,
                options =>
                    options.MapFrom(src =>
                        src.EndDateTime != null ? src.EndDateTime.Value.ToString() : null // custom code
                ))
            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src =>
                        src.OffPercentage != null && (src.StartDateTime <= now && src.EndDateTime >= now)
                        ))
            .ForMember(dest => dest.Count,
                options =>
                    options.MapFrom(src =>
                        src.Count > 3 ? (byte)0 : (byte)src.Count
                    ));
        this.CreateMap<Entities.ProductShortLink, ShowProductShortLinkViewModel>();
        this.CreateMap<Entities.ProductVariant, EditProductVariantViewModel>()
            .ForMember(dest => dest.MainPicture,
                options =>
                    options.MapFrom(src => src.Product.ProductMedia.First().FileName))
            .ForMember(dest => dest.ProductTitle,
                options =>
                    options.MapFrom(src => src.Product.PersianTitle))
            .ForMember(dest => dest.IsDiscountActive,
                options =>
                    options.MapFrom(src => src.OffPercentage != null && (src.StartDateTime <= now && src.EndDateTime >= now)))
            .ForMember(dest => dest.CommissionPercentage,
                options =>
                    options.MapFrom(
                        src => src.Product.Category.CategoryBrands
                            .Select(x => new
                            {
                                x.BrandId,
                                x.CommissionPercentage
                            })
                            .Single(x => x.BrandId == src.Product.BrandId)
                            .CommissionPercentage
                    ));
        this.CreateMap<Entities.ProductVariant, AddEditDiscountViewModel>()
            .ForMember(dest => dest.MainPicture,
                options =>
                    options.MapFrom(src => src.Product.ProductMedia.First().FileName))
            .ForMember(dest => dest.ProductTitle,
                options =>
                    options.MapFrom(src => src.Product.PersianTitle))
            .ForMember(dest => dest.CommissionPercentage,
                options =>
                    options.MapFrom(
                        src => src.Product.Category.CategoryBrands
                            .Select(x => new
                            {
                                x.BrandId,
                                x.CommissionPercentage
                            })
                            .Single(x => x.BrandId == src.Product.BrandId)
                            .CommissionPercentage
                    ));

        this.CreateMap<AddEditDiscountViewModel, Entities.ProductVariant>()
            .ForMember(x => x.Price,
                opt => opt.Ignore())
            .ForMember(x => x.StartDateTime,
                opt => opt.Ignore())
            .ForMember(x => x.EndDateTime,
                opt => opt.Ignore());
        this.CreateMap<Entities.Variant, ShowVariantInEditCategoryVariantViewModel>();
    }
}
