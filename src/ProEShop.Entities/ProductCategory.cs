using ProEShop.Entities.AuditableEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProEShop.Entities;

[Table("ProductCategories")]
public class ProductCategory : IAuditableEntity
{
    public long ProductId { get; set; }
    public long CategoryId { get; set; }
    public Product Product { get; set; } 
    public Category Category { get; set; }
}