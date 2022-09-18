﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProEShop.Entities.AuditableEntity;

namespace ProEShop.Entities;

[Table("Products")]
public class Product : EntityBase, IAuditableEntity
{

    #region Properties
    [Required]
    [MaxLength(200)]
    public string? PersianTitle { get; set; }

    [MaxLength(200)]
    public string? EnglishTitle { get; set; }

    public bool IsFake { get; set; }

    public int PackWeight { get; set; }

    public int PackLength { get; set; }

    public int PackWidth { get; set; }

    public int PackHeight { get; set; }

    [Column(TypeName = "ntext")]
    public string? ShortDescription { get; set; }

    [Column(TypeName = "ntext")]
    public string? SpecialtyCheck { get; set; }

    public long BrandId { get; set; }
    #endregion
    #region Relations

    public ICollection<ProductMedia> ProductMedia { get; set; }

    public ICollection<ProductCategory> ProductCategories { get; set; }

    public ICollection<ProductFeature> ProductFeatures { get; set; }

    public Brand Brand { get; set; }
    #endregion
}