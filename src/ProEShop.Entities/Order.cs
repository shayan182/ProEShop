using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProEShop.Entities.AuditableEntity;
using ProEShop.Entities.Identity;

namespace ProEShop.Entities;

[Table("Orders")]
[Index(nameof(OrderNumber), IsUnique = true)]
public class Order : EntityBase, IAuditableEntity
{
    #region Properties

    public long UserId { get; set; }
    public int OrderNumber { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public long AddressId { get; set; }
    public bool PayFromWallet { get; set; }

    #endregion

    #region Relations

    public User User { get; set; }
    public Address Address { get; set; }

    public ICollection<ParcelPost> ParcelPosts { get; set; }
        = new List<ParcelPost>(); 

    #endregion
}