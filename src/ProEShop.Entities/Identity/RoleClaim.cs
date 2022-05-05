using Microsoft.AspNetCore.Identity;
using ProEShop.Entities.AuditableEntity;
using ProEShop.Entities.Identity;

namespace ProEShop.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<long>, IAuditableEntity
{
    public virtual Role Role { get; set; }
}