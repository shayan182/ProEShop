﻿using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;

namespace ProEShop.Services.Services;

public class CustomGenericService<TEntity> : ICustomGenericService<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entities;

    public CustomGenericService(IUnitOfWork uow)
    {
        _entities = uow.Set<TEntity>();
    }
    public async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    public async Task<TEntity?> FindAsync(params object[] ids)
    {
        return await _entities.FindAsync(ids);
    }
    public void Remove(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public void RemoveRange(List<TEntity> entity)
    {
        _entities.RemoveRange(entity);
    }
}