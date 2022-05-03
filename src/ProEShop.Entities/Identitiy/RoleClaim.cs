using Microsoft.AspNetCore.Identity;

namespace ProEShop.Entities.Identitiy;

public class RoleClaim : IdentityRoleClaim<long>
{
    public virtual Role Role { get; set; }
}
