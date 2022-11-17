using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;

namespace ProEShop.ViewModels.Guarantees;

public class AddGuaranteeViewModel
{
    [PageRemote(PageName = "Index", PageHandler = "CheckForTitle"
        , ErrorMessage = AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Title { get; set; }

    [Display(Name = "تعداد ماه")]
    public byte? MonthsCount { get; set; }

    [Display(Name = "عکس")]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public IFormFile? Picture { get; set; }
}