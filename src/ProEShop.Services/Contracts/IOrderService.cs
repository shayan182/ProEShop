using ProEShop.Entities;

namespace ProEShop.Services.Contracts;

public interface IOrderService: IGenericService<Order>
{
    Task<int> GetOrderNumberForCreateOrderAndPay();
}