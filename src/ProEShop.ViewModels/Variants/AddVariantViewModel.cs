using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;

namespace ProEShop.ViewModels.Variants;

public class AddVariantViewModel
{
    [PageRemote(PageName = "Index", PageHandler = "CheckForValue"
        , ErrorMessage = AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    [Display(Name = "مقدار")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Value { get; set; }

    [Display(Name = "رنگ / اندازه")]
    public bool IsColor { get; set; }

    [PageRemote(PageName = "Index", PageHandler = "CheckForColorCode"
        , ErrorMessage = AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    [Display(Name = "کد رنگی")]
    [LtrDirection]
    public string? ColorCode { get; set; }
}