﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProEShop.Entities.AuditableEntity;

namespace ProEShop.Entities;

[Table("ProductVariants")]
[Index(nameof(ProductVariant.SellerId),
    nameof(ProductVariant.ProductId),
    nameof(ProductVariant.VariantId),
    IsUnique = true)]
[Index(nameof(ProductVariant.VariantCode),IsUnique = true)]

public class ProductVariant : EntityBase, IAuditableEntity
{
    #region Properties
    public long ProductId { get; set; }
    public long? VariantId { get; set; } 
    public long GuaranteeId { get; set; }
    public long SellerId { get; set; }
    public int Price { get; set; }
    public int? OffPrice { get; set; }
    public byte? OffPercentage { get; set; }
    [NotMapped]
    public int FinalPrice => OffPrice ?? Price;
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public int VariantCode { get; set; }
    public int Count { get; set; }
    public short MaxCountInCart { get; set; }    

    #endregion

    #region Relations
    public Product? Product { get; set; }
    public Seller? Seller { get; set; }
    public Variant? Variant { get; set; }
    public Guarantee? Guarantee { get; set; }

    public ICollection<ConsignmentItem> ConsignmentItems { get; set; }
    public ICollection<ProductStock> ProductStocks { get; set; }
    public ICollection<ParcelPostItem> ParcelPostItems { get; set; }

    #endregion
}