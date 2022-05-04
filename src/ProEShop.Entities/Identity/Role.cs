using Microsoft.AspNetCore.Identity;

namespace ProEShop.Entities.Identity;

public class Role : IdentityRole<long>
{
    public Role(string name) : base(name)
    {
    }

    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}