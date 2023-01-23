using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProEShop.Entities.AuditableEntity;

namespace ProEShop.Entities;

[Table("ConsignmentItems")]
[Index(nameof(ConsignmentItem.ProductVariantId),
    nameof(ConsignmentItem.ConsignmentId),IsUnique = true)]
public class ConsignmentItem : EntityBase, IAuditableEntity
{
    #region Properties

    public long ProductVariantId { get; set; }

    public long ConsignmentId { get; set; }

    public int Count { get; set; }

    //Example
    // (VariantCode)--(SellerId)
    // 4--28
    [Required]
    [MaxLength(40)]
    public string? Barcode { get; set; }

    #endregion

    #region Relations

    public ProductVariant? ProductVariant { get; set; }

    public Consignment? Consignment { get; set; }

    #endregion
}