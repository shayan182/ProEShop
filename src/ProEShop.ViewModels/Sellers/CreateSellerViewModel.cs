using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;
using ProEShop.Entities;

namespace ProEShop.ViewModels.Sellers;

public class CreateSellerViewModel
{
    [Display(Name = "نام")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string LastName { get; set; }

    [Display(Name = "کد ملی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(11, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string NationalCode { get; set; }

    [Display(Name = "تاریخ تولد")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public string BirthDate { get; set; }

    [Display(Name = "جنسیت")]
    public Gender? Gender { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [HiddenInput]
    public string PhoneNumber { get; set; }

    public bool IsLegalPerson { get; set; }

    #region legal person

    [Display(Name = "نام شرکت")]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string CompanyName { get; set; }

    [Display(Name = "شماره ثبت شرکت")]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string RegisterNumber { get; set; }

    [Display(Name = "کد اقتصادی")]
    [MaxLength(12, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string EconomicCode { get; set; }

    [Display(Name = "نام افراد دارای حق امضا")]
    [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string SignatureOwners { get; set; }

    [Display(Name = "شناسه ملی")]
    [MaxLength(30, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string NationalId { get; set; }

    [Display(Name = "نوع شرکت")]
    public CompanyType CompanyType { get; set; }

    #endregion

    [Display(Name = "نام فروشگاه")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string ShopName { get; set; }

    [Display(Name = "درباره فروشگاه")]
    public string AboutSeller { get; set; }

    [Display(Name = "لوگو فروشگاه")]
    [IsImage("لوگو فروشگاه")]
    [MaxFileSize("لوگو فروشگاه", 1)]
    public IFormFile? Logo { get; set; }

    /// <summary>
    /// عکس کارت ملی
    /// </summary>
    [Display(Name = "تصویر کارت ملی")]
    [FileRequired("تصویر کارت ملی")]
    [IsImage("تصویر کارت ملی")]
    [MaxFileSize("تصویر کارت ملی", 1)]
    public IFormFile IdCartPicture { get; set; }

    [Display(Name = "شماره شبا")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(24, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string ShabaNumber { get; set; }

    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(11, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Telephone { get; set; }

    [Display(Name = "آدرس وبسایت")]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Website { get; set; }

    public long ProvinceId { get; set; }

    public long CityId { get; set; }

    [Display(Name = "آدرس کامل")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Address { get; set; }
    
    [Display(Name = "کد پستی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(10, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [RegularExpression(@"^[\d]{10}",ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public string PostalCode { get; set; }

    public List<SelectListItem> Provinces { get; set; }
    [Display(Name = "قوانین را قبول دارم.")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public bool AcceptToTheTerms { get; set; }
}
