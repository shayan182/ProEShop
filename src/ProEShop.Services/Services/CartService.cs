using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Carts;
using ProEShop.ViewModels.Products;

namespace ProEShop.Services.Services;

public class CartService : CustomGenericService<Cart>, ICartService
{
    private readonly DbSet<Cart> _carts;
    private readonly IMapper _mapper;

    public CartService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _carts = uow.Set<Cart>();
    }

    public Task<List<ProductVariantInCartForProductInfoViewModel>> GetProductVariantsInCart(List<long> productVariantIds, long userId)
    {
        return _mapper.ProjectTo<ProductVariantInCartForProductInfoViewModel>(
            _carts.Where(x=>x.UserId == userId)
                .Where(x=>productVariantIds.Contains(x.ProductVariantId))).ToListAsync();
    }

    public Task<List<ShowCartInDropDownViewModel>> GetCartsForDropDown(long userId)
    {
        return _carts.AsNoTracking()
            .Where(x => x.UserId == userId)
            .ProjectTo<ShowCartInDropDownViewModel>(
                configuration:_mapper.ConfigurationProvider, parameters:new {now  = DateTime.Now}
            ).ToListAsync();
    }
}
