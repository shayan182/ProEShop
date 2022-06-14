using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;

namespace ProEShop.ViewModels.Identity;

public class LoginWithPhoneNumberViewModel
{
    [Display(Name = "کد فعال سازی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [StringLength(6, MinimumLength = 6, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public string ActivationCode { get; set; }

    [HiddenInput]
    public string PhoneNumber { get; set; }

    public byte SendSmsLastTimeMinute { get; set; }

    public byte SendSmsLastTimeSecond { get; set; }
}