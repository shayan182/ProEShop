﻿using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;

namespace ProEShop.Services.Services;

public class ProvinceAndCityService : GenericService<ProvinceAndCity> , IProvinceAndCityService
{
    private readonly DbSet<ProvinceAndCity> _provinceAndCities;
    public ProvinceAndCityService(IUnitOfWork uow) : base(uow)
    {
        _provinceAndCities = uow.Set<ProvinceAndCity>();
    }

    public Task<Dictionary<long, string>> GetProvincesToShowInSelectBoxAsync()
    {
        return _provinceAndCities.Where(x => x.ParentId == null)
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, x => x.Title);
    }
    public Task<Dictionary<long, string>> GetCitiesToShowInSelectBoxAsync()
    {
        return _provinceAndCities.Where(x => x.ParentId != null)
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, x => x.Title);
    }
    public async Task<Dictionary<long, string>> GetCitiesByProvinceIdToShowInSelectBoxAsync(long provinceId)
    {
        return await _provinceAndCities.Where(x => x.ParentId == provinceId)
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Id, x => x.Title);
    }

    public async Task<(long, long)> GetForSeedData()
    {
        var province = await _provinceAndCities.Where(x => x.ParentId == null)
            .Select(x=>new
            {
                x.Title,
                x.Id
            }).AsNoTracking().SingleAsync(x => x.Title == "تهران");

        var city = await _provinceAndCities.Where(x => x.ParentId != null)
            .Select(x => new
            {
                x.Title,
                x.Id
            }).AsNoTracking().SingleAsync(x => x.Title == "تهران");
        return (province.Id, city.Id);
    }
}