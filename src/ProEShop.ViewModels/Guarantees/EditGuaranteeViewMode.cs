using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;

namespace ProEShop.ViewModels.Guarantees;

public class EditGuaranteeViewMode
{
    [HiddenInput]
    public long Id { get; set; }

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
    [IsImage]
    [MaxFileSize(2)]
    public IFormFile? NewPicture { get; set; }

    [Display(Name = "عکس از قبل بارگذاری شده")]
    public string? Picture { get; set; }


}