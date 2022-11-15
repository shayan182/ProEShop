using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;

namespace ProEShop.ViewModels.Variants;

public class EditVariantViewMode
{
    [HiddenInput]
    public long Id { get; set; }

    [PageRemote(PageName = "Index", PageHandler = "CheckForValueOnEdit"
        , ErrorMessage = AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id))]
    [Display(Name = "مقدار")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage )]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Value { get; set; }

    [Display(Name = "رنگ / اندازه")]
    public bool IsColor { get; set; }

    [PageRemote(PageName = "Index", PageHandler = "CheckForColorCodeOnEdit"
        , ErrorMessage = AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id))]
    [Display(Name = "کد رنگی")]
    [LtrDirection]
    public string? ColorCode { get; set; }
}