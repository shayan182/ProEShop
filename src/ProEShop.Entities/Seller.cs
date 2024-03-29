﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProEShop.Entities.AuditableEntity;
using ProEShop.Entities.Identity;

namespace ProEShop.Entities;

[Table("Sellers")]
[Index(nameof(Seller.ShabaNumber), IsUnique = true)]
[Index(nameof(Seller.ShopName), IsUnique = true)]
[Index(nameof(Seller.SellerCode), IsUnique = true)]
public class Seller : EntityBase, IAuditableEntity
{
    #region Properties

    [Display(Name = "شناسه")]
    public long? UserId { get; set; }

    [Display(Name = "شخص حقوقی / شخص حقیقی")]
    public bool? IsRealPerson { get; set; }

    #region Legal person

    [Display(Name = "نام شرکت")]
    [MaxLength(200)]
    public string? CompanyName { get; set; }

    [Display(Name = "شماره ثبت شرکت")]
    [MaxLength(100)]
    public string? RegisterNumber { get; set; }

    [Display(Name = "کد اقتصادی")]
    [MaxLength(12)]
    public string? EconomicCode { get; set; }

    [Display(Name = "نام افراد دارای حق امضا")]
    [MaxLength(300)]
    public string? SignatureOwners { get; set; }

    [Display(Name = "شناسه ملی")]
    [MaxLength(30)]
    public string? NationalId { get; set; }

    [Display(Name = "نوع شرکت")]
    public CompanyType? CompanyType { get; set; }
    #endregion

    [Display(Name = "کد فروشنده")]
    public int? SellerCode { get; set; }

    [Display(Name = "نام فروشگاه")]
    [Required]
    [MaxLength(200)]
    public string? ShopName { get; set; }

    [Display(Name = "درباره فروشگاه")]
    [Column(TypeName = "ntext")]
    public string? AboutSeller { get; set; }

    [Display(Name = "لوگو فروشگاه")]
    [MaxLength(50)]
    public string? Logo { get; set; }

    /// <summary>
    /// عکس کارت ملی
    /// </summary>
    [Display(Name = "تصویر کارت ملی")]
    [Required]
    [MaxLength(50)]
    public string IdCartPicture { get; set; }

    [Display(Name = "شماره شبا")]
    [Required]
    [MaxLength(24)]
    public string? ShabaNumber { get; set; }

    [Display(Name = "شماره تلفن ثابت")]
    [Required]
    [MaxLength(11)]
    public string TelePhone { get; set; }

    [Display(Name = "آدرس وبسایت")]
    [MaxLength(200)]
    public string? Website { get; set; }

    [Display(Name = "آدرس کامل")]
    [Required]
    [MaxLength(300)]
    public string Address { get; set; }

    [Display(Name = "کد پستی")]
    [Required]
    [MaxLength(10)]
    public string PostalCode { get; set; }
    
    [MaxLength(100)]
    public string? Location { get; set; }

    [Display(Name = "وضعیت مدارک")]

    public DocumentStatus? DocumentStatus { get; set; }

    public bool? IsActive { get; set; } = true;

    [Display(Name = "تاریخ ثبت نام")]
    public DateTime CreatedDateTime { get; set; }
    public long? ProvinceId { get; set; }
    public long? CityId { get; set; }
    [Display(Name = "دلایل رد مدارک فروشنده")]
    [Column(TypeName = "ntext")]
    public string? RejectReason { get; set; }

    #endregion

    #region Relations

    public User User { get; set; }
    public ProvinceAndCity? Province { get; set; }

    public ProvinceAndCity? City { get; set; }

    public ICollection<Product> Products { get; set; }
    public ICollection<ProductVariant>? ProductVariants { get; set; }
    #endregion
}

public enum DocumentStatus : byte
{
    [Display(Name = "در انتظار تایید اولیه")]
    AwaitingInitialApproval,

    [Display(Name = "تایید شده")]
    Confirmed,

    [Display(Name = "رد شده در حالت اولیه")]
    Rejected,

    [Display(Name = "در انتظار تایید فروشنده سیستم")]
    AwaitingApprovalSystemSeller,

    [Display(Name = "رد شده برای فروشنده  سیستم")]
    RejectedSystemSeller
}
public enum Gender : byte
{
    [Display(Name = "آقا")]
    Man,
    [Display(Name = "خانوم")]
    Woman
}

public enum CompanyType : byte
{
    [Display(Name = "سهامی عام")]
    PublicStock,

    [Display(Name = "سهامی خاص")]
    PrivateEquity,

    [Display(Name = "مسئولیت محدود")]
    LimitedResponsibility,

    [Display(Name = "تعاونی")]
    Cooperative,

    [Display(Name = "تضامنی")]
    Solidarity
}