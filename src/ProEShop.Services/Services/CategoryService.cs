using AutoMapper;
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
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork uow,
        IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _categories = uow.Set<Category>();
    }
    public async Task<ShowCategoriesViewModel> GetCategories(ShowCategoriesViewModel model)
    {
        var categories = _categories.AsNoTracking().AsQueryable();

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
        return await _categories.AsNoTracking()
            .Where(x => x.Id != id || id == null)
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

    public async Task<EditCategoryViewModel?> GetForEdit(long id)
    {
        return await _mapper.ProjectTo<EditCategoryViewModel>(_categories)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<List<ShowCategoryForCreateProductViewModel>>> GetCategoriesForCreateProduct(long[] selectedCategoriesIds)
    {
        var result = new List<List<ShowCategoryForCreateProductViewModel>>
        {
            await _categories.Where(x => x.ParentId == null)
                .Select(x => new ShowCategoryForCreateProductViewModel()
                {
                    Title = x.Title,
                    Id = x.Id,
                    HasChild = x.Categories.Any()
                }).AsNoTracking().ToListAsync()
        };
        for (int counter = 0; counter < selectedCategoriesIds.Length; counter++)
        {

            var selectedCategoryId = selectedCategoriesIds[counter];
            result.Add(await _categories.Where(x => x.ParentId == selectedCategoryId)
                .Select(x => new ShowCategoryForCreateProductViewModel()
                {
                    Title = x.Title,
                    Id = x.Id,
                    HasChild = x.Categories.Any()
                }).AsNoTracking().ToListAsync());

        }

        return result;
    }

    public async Task<List<string>> GetCategoryBrands(long categoryId)
    {
        return await _categories
            .Where(x => x.Id == categoryId)
            .SelectMany(x => x.CategoryBrands)
            .Select(x => x.Brand.TitleFa + " " + x.Brand.TitleEn)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryWithItsBrands(long categoryId)
    {
        return await _categories
            .Include(x => x.CategoryBrands)
            .SingleOrDefaultAsync(x => x.Id == categoryId);
    }

    public async Task<Dictionary<long, string>> GetBrandsByCategoryId(long categoryId)
    {
        return await _categories
            .SelectMany(x => x.CategoryBrands)
            .Where(x => x.CategoryId == categoryId)
            .Include(x => x.Brand)
            .ToDictionaryAsync(x => x.BrandId ,x=>x.Brand.TitleFa + " " + x.Brand.TitleEn);
    }

    public async Task<bool> CanAddFakeProduct(long categoryId)
    {
        var category = await _categories
            .Select(x => new
            {
                x.Id,
                x.CanAddFakeProduct
            }).SingleOrDefaultAsync(x => x.Id == categoryId);
        return category?.CanAddFakeProduct ?? false;
    }
}
