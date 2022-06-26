using System.ComponentModel.DataAnnotations;
using ProEShop.Entities.AuditableEntity;

namespace ProEShop.Entities;

public class Product : EntityBase, IAuditableEntity
{

    #region Properties
    [Required]
    [MaxLength(300)]
    public string Title { get; set; }

    public long CategoryId { get; set; }
    #endregion
    #region Relations

    public Category Category { get; set; }
    #endregion
}