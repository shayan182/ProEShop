using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.CategoryFeatures;

namespace ProEShop.Services.Services;

public class UserProductFavoriteService : CustomGenericService<UserProductFavorite>, IUserProductFavoriteService
{
    private readonly DbSet<UserProductFavorite> _userProductFavorites;
    private readonly IMapper _mapper;

    public UserProductFavoriteService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _userProductFavorites = uow.Set<UserProductFavorite>();
    }
   
}