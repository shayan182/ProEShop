using Microsoft.AspNetCore.Identity;

namespace ProEShop.Entities.Identitiy
{
    public class UserLogin : IdentityUserLogin<long>
    {
        public virtual User User { get; set; }

    }
}
