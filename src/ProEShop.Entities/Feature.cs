using ProEShop.Entities.AuditableEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProEShop.Entities;

[Table("Features")]
public class Feature : EntityBase, IAuditableEntity
{
    #region Properties

    [Required]
    [MaxLength(150)]
    public string Title { get; set; }
    #endregion
    #region Relations
    public ICollection<CategoryFeature> categoryFeatures { get; set; }
    #endregion
}