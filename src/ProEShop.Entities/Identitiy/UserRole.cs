using Microsoft.AspNetCore.Identity;

namespace ProEShop.Entities.Identitiy
{
    public class UserRole : IdentityUserRole<long>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }

    }
}
