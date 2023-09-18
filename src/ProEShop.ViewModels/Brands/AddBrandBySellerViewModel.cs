using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;
namespace ProEShop.ViewModels.Brands;

public class AddBrandBySellerViewModel
{
    [HiddenInput]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long CategoryId { get; set; }

    [PageRemote(PageName = "Create", PageHandler = "CheckForTitleFa"
        , ErrorMessage = AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    [Display(Name = "نام فارسی برند")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string TitleFa { get; set; }

    [PageRemote(PageName = "Create", PageHandler = "CheckForTitleEn"
        , ErrorMessage = AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    [Display(Name = "نام انگلیسی برند")]
    [LtrDirection]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string TitleEn { get; set; }

    [Display(Name = "شرح برند")]
    [MakeTinyMceRequired]
    public string? Description { get; set; }

    [Display(Name = "نوع برند")]
    public bool IsIranianBrand { get; set; }

    [Display(Name = "لوگوی برند")]
    [IsImage]
    [MaxFileSize(3)]
    public IFormFile? LogoPicture { get; set; }

    [Display(Name = "برگه ثبت برند")]
    [IsImage]
    [MaxFileSize(3)]
    public IFormFile? BrandRegistrationPicture { get; set; }

    [Display(Name = "لینک سایت قوه قضاییه")]
    [LtrDirection]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? JudiciaryLink { get; set; }

    [Display(Name = "لینک سایت معتبر خارجی")]
    [LtrDirection]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? BrandLinkEn { get; set; }
}