using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEShop.Entities.Identitiy
{
    public class UserClaim : IdentityUserClaim<long>
    {
        public virtual User User { get; set; }

    }
}
