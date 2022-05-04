﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities.Identitiy;
using ProEShop.Services.Contracts.Identity;

namespace ProEShop.Services.Implements.Identity;

public class ApplicationRoleStore :
    RoleStore<Role,ApplicationDbContext,long,UserRole,RoleClaim>, IApplicationRoleStore
{
    public ApplicationRoleStore(
        IUnitOfWork uow,
        IdentityErrorDescriber describer = null) 
        : base((ApplicationDbContext)uow, describer)
    {

    }
    #region Custom Class



    #endregion

}