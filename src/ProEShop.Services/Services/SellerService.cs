using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;

namespace ProEShop.Services.Services;

public class SellerService : GenericService<Seller>, ISellerService
{
    public SellerService(IUnitOfWork uow) : base(uow)
    {
    }
}
