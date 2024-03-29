﻿using AutoMapper;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProEShop.Common.Helpers;
using ProEShop.DataLayer.Context;
using ProEShop.Entities.Identity;
using ProEShop.Services.Contracts.Identity;
using ProEShop.ViewModels.Sellers;

namespace ProEShop.Services.Services.Identity;

public class ApplicationUserManager :
    UserManager<User>, IApplicationUserManager
{
    private readonly IMapper _mapper;
    private readonly DbSet<User> _users;
    public ApplicationUserManager(
        IApplicationUserStore store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators, 
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<ApplicationUserManager> logger,
        IUnitOfWork uow, IMapper mapper)
        : base((UserStore<User, Role, ApplicationDbContext, long, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>)store,
            optionsAccessor, passwordHasher, userValidators, 
            passwordValidators, keyNormalizer, errors, services, logger)
    {
        _mapper = mapper;
        _users = uow.Set<User>();
    }
    #region Custom Class
    public async Task<DateTime?> GetSendSmsLastTimeAsync(string phoneNumber)
    {
        var result = await _users.Select(x => new
            {
                x.UserName,
                x.SendSmsLastTime
            })
            .SingleOrDefaultAsync(x => x.UserName == phoneNumber);
        return result?.SendSmsLastTime;
    }

    public async Task<bool> CheckForUserIsSeller(string phoneNumber)
    {
       return await _users.Where(x => x.UserName == phoneNumber)
            .Where(x => x.UserRoles.All(r => r.Role.Name != ConstantRoles.Seller))
            .AnyAsync(x => x.IsSeller == true);
    }

    public async Task<CreateSellerViewModel?> GetUserInfoForCreateSeller(string phoneNumber)
    {
        var result = await _mapper.ProjectTo<CreateSellerViewModel>(_users)
            .SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        if (result?.BirthDate != null)
        {
            var parsedDateTime = DateTime.Parse(result.BirthDate);
            result.BirthDateEn = parsedDateTime.ToString("yyyy-MM-dd");
            result.BirthDate = parsedDateTime.ToShortPersianDate().ToPersianNumbers();
        }
        return result;
    }
    public async Task<User?> GetUserForCreateSeller(string userName)
    {
        return await _users.Where(x => x.IsSeller == true )
            .Where(x => x.UserRoles.All(r => r.Role.Name != ConstantRoles.Seller))
            .SingleOrDefaultAsync(x => x.UserName == userName);
    }
    #endregion


}