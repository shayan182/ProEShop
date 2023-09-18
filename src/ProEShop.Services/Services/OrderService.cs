using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Carts;

namespace ProEShop.Services.Services;

public class OrderService : GenericService<Order>, IOrderService
{
    private readonly DbSet<Order> _orders;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _orders = uow.Set<Order>();
    }

    public async Task<int> GetOrderNumberForCreateOrderAndPay()
    {
        //custom
        var lastOrderNumber = await _orders.OrderByDescending(x => x.Id)
            .Select(x => x.OrderNumber)
            .FirstOrDefaultAsync();

        return lastOrderNumber + 1;
    }
}
