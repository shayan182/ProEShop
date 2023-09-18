using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProEShop.Entities.AuditableEntity;
using ProEShop.Entities.Enums;
using ProEShop.Entities.Identity;

namespace ProEShop.Entities;

/// <summary>
/// مرسوله
/// هر سفارش میتواند شامل چندین مرسوله باشد
/// </summary>
[Table("ParcelPosts")]
[Index(nameof(PostTrackingCode), IsUnique = true)]
public class ParcelPost : EntityBase, IAuditableEntity
{
    #region Properties

    public long OrderId { get; set; }

    public Dimension Dimension { get; set; }

    public OrderStatus Status { get; set; }

    [MaxLength(30)]
    public string? PostTrackingCode { get; set; }

    public int ShippingPrice { get; set; }

    #endregion

    #region Relations

    public Order Order { get; set; }

    public ICollection<ParcelPostItem> ParcelPostItems { get; set; }
        = new List<ParcelPostItem>();

    #endregion
}