﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProEShop.Entities.AuditableEntity;
using ProEShop.Entities.Enums;

namespace ProEShop.Entities;

[Table("Products")]
[Index(nameof(ProductCode), IsUnique = true)]
public class Product : EntityBase, IAuditableEntity
{

    #region Properties
    [Required]
    [MaxLength(200)]
    public string? PersianTitle { get; set; }

    [MaxLength(200)]
    public string? EnglishTitle { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Slug { get; set; } 

    public bool IsFake { get; set; }

    public int PackWeight { get; set; }

    public int PackLength { get; set; }

    public int PackWidth { get; set; }

    public int PackHeight { get; set; }

    [Column(TypeName = "ntext")]
    public string? ShortDescription { get; set; }

    [Column(TypeName = "ntext")]
    public string? SpecialtyCheck { get; set; }

    public ProductStatus Status { get; set; }
    public ProductStockStatus ProductStockStatus { get; set; }

    [Column(TypeName = "ntext")]
    public string? RejectReason { get; set; }

    public int ProductCode { get; set; }
    public long BrandId { get; set; }
    public long SellerId { get; set; }

    [ForeignKey(nameof(Category))]
    public long MainCategoryId { get; set; }
    public long ProductShortLinkId { get; set; }
    public Dimension Dimension { get; set; }
    #endregion
    #region Relations

    public ICollection<ProductMedia> ProductMedia { get; set; }
        = new List<ProductMedia>();

    public ICollection<ProductCategory> ProductCategories { get; set; }
        = new List<ProductCategory>();

    public ICollection<ProductFeature> ProductFeatures { get; set; }
        = new List<ProductFeature>();

    public Brand? Brand { get; set; }
    public Seller? Seller { get; set; }
    public Category? Category { get; set; }
    public ICollection<ProductVariant>? ProductVariants { get; set; }
    public ICollection<ProductComment>? ProductComments { get; set; }
    public ICollection<UserProductFavorite>? UserProductFavorites { get; set; }
    public ProductShortLink? ProductShortLink { get; set; }
    #endregion
    
}

public enum ProductStockStatus : byte
{
    [Display(Name = "جدید")]
    New,
    [Display(Name = "موجود")]
    Available,
    [Display(Name = "ناموجود")]
    Unavailable,
}
public enum ProductStatus : byte
{
    [Display(Name = "در انتظار تایید اولیه")]
    AwaitingInitialApproval,

    [Display(Name = "تایید شده")]
    Confirmed,

    [Display(Name = "رد شده در حالت اولیه")]
    Rejected
}