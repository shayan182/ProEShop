using Microsoft.AspNetCore.Identity;

namespace ProEShop.Entities.Identitiy;

public class UserClaim : IdentityUserClaim<long>
{
    public virtual User User { get; set; }

}
