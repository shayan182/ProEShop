﻿using Microsoft.EntityFrameworkCore;
using ProEShop.Common.Helpers;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;

namespace ProEShop.Services.Services;

public class SellerService : GenericService<Seller>, ISellerService
{
    private readonly DbSet<Seller> _sellers;
    public SellerService(IUnitOfWork uow) : base(uow)
    {
        _sellers = uow.Set<Seller>();
    }

    public override async Task<DuplicateColumns> AddAsync(Seller entity)
    {
        var result = new List<string>();
        if (await _sellers.AnyAsync(x => x.ShopName == entity.ShopName))
            result.Add(nameof(entity.ShopName));
        if (await _sellers.AnyAsync(x => x.ShabaNumber == entity.ShabaNumber))
            result.Add(nameof(entity.ShabaNumber));
        if (await _sellers.AnyAsync(x => x.SellerCode == entity.SellerCode))
            result.Add(nameof(entity.SellerCode));
        if (!result.Any())
            await base.AddAsync(entity);
        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };
    }

    public async Task<int> GetSellerCodeForCreateSeller()
    {
        var latestSellerCode = await _sellers.OrderByDescending(x => x.Id)
            .Select(x => x.SellerCode)
            .FirstOrDefaultAsync();
        return (int)(latestSellerCode + 1);
    }
}
