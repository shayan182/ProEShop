﻿using System.Collections.Generic;
using System.Threading.Tasks;
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
        var paginationResult = await GenericPaginationAsync(categories, model.Pagination);

        return new()
        {
            Categories = await paginationResult.Query
            .Select(x => new ShowCategoryViewModel()
            {
                Title = x.Title,
                ShowInMenus = x.ShowInMenus,
                Parent = x.ParentId != null ? x.Parent.Title : "دسته اصلی",
                Slug = x.Slug,
                Picture = x.Picture ?? "بدون عکس"
            })
        .ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public Dictionary<long, string> GetCategoriesToShowInSelelctBox()
    {
        return _categories.ToDictionary(x => x.Id, x => x.Title);
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
}
