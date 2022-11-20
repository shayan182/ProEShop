using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;

namespace ProEShop.Services.Services;

public class ProductVariantService : GenericService<ProductVariant>, IProductVariantService
{
    private readonly DbSet<ProductVariant> _productVariants;

    public ProductVariantService(IUnitOfWork uow)
        : base(uow)
    {
        _productVariants = uow.Set<ProductVariant>();
    }
}
