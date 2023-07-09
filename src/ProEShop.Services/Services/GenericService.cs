using Microsoft.EntityFrameworkCore;
using ProEShop.Common.Helpers;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels;

namespace ProEShop.Services.Services;

public abstract class GenericService<TEntity> : IGenericService<TEntity> where TEntity : EntityBase, new()
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<TEntity> _entities;

    public GenericService(IUnitOfWork uow)
    {
        _uow = uow;
        _entities = uow.Set<TEntity>();
    }
    public virtual async Task<DuplicateColumns> AddAsync(TEntity entity)
    {

        await _entities.AddAsync(entity);
        return new DuplicateColumns();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entity)
    {
        await _entities.AddRangeAsync(entity);
    }

    public virtual Task<DuplicateColumns> Update(TEntity entity)
    {
        _entities.Update(entity);
        return Task.FromResult(new DuplicateColumns());
    }

    public void Remove(TEntity entity)
        => _entities.Remove(entity);

    public void Remove(long id)
    {
        var tEntity = new TEntity();
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty is null)
            throw new Exception("The entity doesn't have Id field!");
        idProperty.SetValue(tEntity, id, null);
        _uow.MarkAsDeleted(tEntity);
    }

    public async Task<TEntity?> FindByIdAsync(long id)
        => await _entities.FindAsync(id);

    public Task<TEntity?> FindByIdWithIncludeAsync(long id,params string[] includes)
    {
        var query = _entities.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> IsExistsByIdAsync(long id)
        => await _entities.AsNoTracking().AnyAsync(x => x.Id == id);

    public async Task<PaginationResultViewModel<T>> GenericPaginationAsync<T>(IQueryable<T> items, PaginationViewModel pagination)
    {
        if (pagination.CurrentPage < 0)
            pagination.CurrentPage = 1;

        var take = pagination.PageCount switch
        {
            PageCount.Fifty => 50,
            PageCount.TwentyFive => 25,
            PageCount.Hundred => 100,
            _ => 10
        };
        var itemCount = await items.LongCountAsync();
        var pageCount = (int)Math.Ceiling((decimal)itemCount / take);

        if (pageCount <= 0)
            pageCount = 1;
        if (pagination.CurrentPage > pageCount)
            pagination.CurrentPage = pageCount;

        var skip = (pagination.CurrentPage - 1) * take;



        pagination.PagesCount = pageCount;
        return new()
        {
            Pagination = pagination,
            Query = items.Skip(skip).Take(take)
        };
    }

    public void SoftDelete(TEntity entity)
    {
        entity.IsDeleted = true;
    }
    public void Restore(TEntity entity)
    {
        entity.IsDeleted = false;
    }

    public async Task<bool> AnyAsync() => await _entities.AsNoTracking().AnyAsync();

    public async Task<bool> IsExistsBy(string propertyToFilter, object propertyValue, long? id = null)
    {
        var exp = ExpressionHelpers.CreateExpression<TEntity>(propertyToFilter, propertyValue);
        return await _entities
            .Where(x => id == null || x.Id != id) // For Edit Methods
            .AsNoTracking()
            .AnyAsync(exp);
    }
}