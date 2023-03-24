using ProEShop.Entities;

namespace ProEShop.Services.Contracts;

public interface IProductShortLinkService : IGenericService<ProductShortLink>
{
    Task<ProductShortLink> GetProductShortLinkForCreateProduct();

}