using ProEShop.Entities;

namespace ProEShop.Services.Contracts;

public interface IProvinceAndCityService : ICustomGenericService<ProvinceAndCity>
{
    Task<Dictionary<long, string>> GetProvincesToShowInSelectBoxAsync();
}