using ProEShop.Entities;
using ProEShop.ViewModels.ProductShortLinks;

namespace ProEShop.Services.Contracts;  

public interface IProductShortLinkService : IGenericService<ProductShortLink>
{
    Task<ProductShortLink> GetProductShortLinkForCreateProduct();
    Task<ShowProductShortLinksViewModel> GetProductShortLinks(ShowProductShortLinksViewModel model);
    Task<ProductShortLink?> GetForDelete(long id);
}