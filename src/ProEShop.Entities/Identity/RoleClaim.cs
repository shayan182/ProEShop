using Microsoft.AspNetCore.Identity;
using ProEShop.Entities.Identity;

namespace ProEShop.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<long>
{
    public virtual Role Role { get; set; }
}