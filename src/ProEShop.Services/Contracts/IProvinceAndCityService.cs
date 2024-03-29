﻿using ProEShop.Entities;

namespace ProEShop.Services.Contracts;

public interface IProvinceAndCityService : IGenericService<ProvinceAndCity>
{
    Task<Dictionary<long, string>> GetProvincesToShowInSelectBoxAsync();
    Task<Dictionary<long, string>> GetCitiesToShowInSelectBoxAsync();
    Task<Dictionary<long, string>> GetCitiesByProvinceIdToShowInSelectBoxAsync(long provinceId);
    Task<(long, long)> GetForSeedData();
}