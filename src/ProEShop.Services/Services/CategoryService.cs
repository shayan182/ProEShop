using Microsoft.EntityFrameworkCore;
using ProEShop.Common.Helpers;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Categories;

namespace ProEShop.Services.Services;

public class CategoryService : GenericService<Category>, ICategoryService
{
    private readonly DbSet<Category> _categories;
    public CategoryService(IUnitOfWork uow)
        : base(uow)
    {
        _categories = uow.Set<Category>();
    }
    public async Task<ShowCategoriesViewModel> GetCategories(ShowCategoriesViewModel model)
    {
        var categories = _categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.SearchCategories.Title))
            categories = categories.Where(x => x.Title.Contains(model.SearchCategories.Title.Trim()));

        if (!string.IsNullOrWhiteSpace(model.SearchCategories.Slug))
            categories = categories.Where(x => x.Slug.Contains(model.SearchCategories.Slug.Trim()));

        switch (model.SearchCategories.DeletedStatus)
        {
            case ViewModels.DeletedStatus.True:
                break;
            case ViewModels.DeletedStatus.OnlyDeleted:
                categories = categories.Where(x => x.IsDeleted);
                break;
            default:
                categories = categories.Where(x => !x.IsDeleted);
                break;
        }

        switch (model.SearchCategories.ShowInMenusStatus)
        {
            case ShowInMenusStatus.True:
                categories = categories.Where(x => x.ShowInMenus);
                break;
            case ShowInMenusStatus.False:
                categories = categories.Where(x => !x.ShowInMenus);
                break;
            default:
                break;
        }

        categories = categories.CreateOrderByExpression(model.SearchCategories.Sorting.ToString(),
            model.SearchCategories.SortingOrder.ToString());

        var paginationResult = await GenericPaginationAsync(categories, model.Pagination);

        return new()
        {
            Categories = await paginationResult.Query
            .Select(x => new ShowCategoryViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                ShowInMenus = x.ShowInMenus,
                Parent = x.ParentId != null ? x.Parent.Title : "دسته اصلی",
                Slug = x.Slug,
                Picture = x.Picture ?? "بدون عکس",
                IsDeleted = x.IsDeleted
            })
        .ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public async Task<Dictionary<long, string>> GetCategoriesToShowInSelectBoxAsync(long? id = null)
    {
        return await _categories
            .Where(x=>x.Id != id || id == null)
            .ToDictionaryAsync(x => x.Id, x => x.Title);
    }

    public override async Task<DuplicateColumns> AddAsync(Category entity)
    {
        var result = new List<string>();
        if (await _categories.AnyAsync(x => x.Title == entity.Title))
            result.Add(nameof(entity.Title));
        if (await _categories.AnyAsync(x => x.Slug == entity.Slug))
            result.Add(nameof(entity.Slug));
        if (!result.Any())
            await base.AddAsync(entity);
        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };
    }
    public override async Task<DuplicateColumns> Update(Category entity)
    {
        var category = _categories.Where(x => x.Id != x.Id);
        var result = new List<string>();
        if (await category.AnyAsync(x => x.Title == entity.Title))
            result.Add(nameof(entity.Title));
        if (await category.AnyAsync(x => x.Slug == entity.Slug))
            result.Add(nameof(entity.Slug));
        if (!result.Any())
            await base.Update(entity);
        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };
    }

    public async Task<EditCategoryViewModel> GetForEdit(long id)
    {
        return await _categories.Select(x => new EditCategoryViewModel()
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            ParentId = x.ParentId,
            SelectedPicture = x.Picture,
            Slug = x.Slug,
            ShowInMenus = x.ShowInMenus
        }).SingleOrDefaultAsync(x => x.Id == id);
    }

    
}
