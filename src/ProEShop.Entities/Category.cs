using Microsoft.EntityFrameworkCore;
using ProEShop.Entities.AuditableEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProEShop.Entities;

[Index(nameof(Category.Slug), IsUnique = true)]
[Index(nameof(Category.Title), IsUnique = true)]
[Table("Categories")]
public class Category : EntityBase, IAuditableEntity
{
    #region Properties

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Column(TypeName ="ntext")]
    public string? Description { get; set; }

    [Required]
    [MaxLength(130)]
    public string Slug { get; set; }

    [MaxLength(50)]
    [AllowNull]
    public string? Picture { get; set; }

    public long? ParentId { get; set; }

    public bool ShowInMenus { get; set; }

    #endregion

    #region Relations

    public Category Parent { get; set; }

    public ICollection<Category> Categories { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<CategoryFeature> categoryFeatures { get; set; }

    #endregion
}