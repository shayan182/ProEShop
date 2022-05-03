using Microsoft.AspNetCore.Identity;

namespace ProEShop.Entities.Identitiy;

public class UserToken : IdentityUserToken<long>
{
    public virtual User User { get; set; }

}
