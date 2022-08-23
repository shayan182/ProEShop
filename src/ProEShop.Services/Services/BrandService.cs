using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.Common.Helpers;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels;
using ProEShop.ViewModels.Brands;

namespace ProEShop.Services.Services;

public class BrandService : GenericService<Brand>, IBrandService
{
    private readonly DbSet<Brand> _brands;
    private readonly IMapper _mapper;

    public BrandService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _brands = uow.Set<Brand>();
    }

    public async Task<ShowBrandsViewModel> GetBrands(ShowBrandsViewModel model)
    {
        var brands = _brands.AsNoTracking().AsQueryable();


        brands = ExpressionHelpers.CreateSearchExpressions(brands, model.SearchBrands);

        

        #region OrderBy


        if (model.SearchBrands.Sorting == SortingBrands.BrandLinkEn)
        {
            if (model.SearchBrands.SortingOrder == SortingOrder.Asc)
            {
                brands = brands.OrderBy(x => x.BrandLinkEn.Substring(
                    x.BrandLinkEn.StartsWith("https://") ? 8 : 7
                ));
            }
            else
            {
                brands = brands.OrderByDescending(x => x.BrandLinkEn.Substring(
                    x.BrandLinkEn.StartsWith("https://") ? 8 : 7
                ));
            }
        }
        else if (model.SearchBrands.Sorting == SortingBrands.JudiciaryLink)
        {
            if (model.SearchBrands.SortingOrder == SortingOrder.Asc)
            {
                brands = brands.OrderBy(x => x.JudiciaryLink.Substring(
                    x.JudiciaryLink.StartsWith("https://") ? 8 : 7
                ));
            }
            else
            {
                brands = brands.OrderByDescending(x => x.JudiciaryLink.Substring(
                    x.JudiciaryLink.StartsWith("https://") ? 8 : 7
                ));
            }
        }
        else
        {
            brands = brands.CreateOrderByExpression(model.SearchBrands.Sorting.ToString(),
                model.SearchBrands.SortingOrder.ToString());
        }

        #endregion

        var paginationResult = await GenericPaginationAsync(brands, model.Pagination);


        return new()
        {
            Brands = await _mapper.ProjectTo<ShowBrandViewModel>(
                    paginationResult.Query
                ).ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public async Task<EditBrandViewMode?> GetForEdit(long id)
    {
        return await _mapper.ProjectTo<EditBrandViewMode>(
            _brands
        ).SingleOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<DuplicateColumns> AddAsync(Brand entity)
    {
        var result = new List<string>();

        if (await _brands.AnyAsync(x => x.TitleFa == entity.TitleFa))
            result.Add(nameof(Brand.TitleFa));

        if (await _brands.AnyAsync(x => x.TitleEn == entity.TitleEn))
            result.Add(nameof(Brand.TitleEn));

        if (!result.Any())
            await base.AddAsync(entity);
        return new(!result.Any())
        {
            Columns = result
        };
    }

    public override async Task<DuplicateColumns> Update(Brand entity)
    {
        var category = _brands.Where(x => x.Id != x.Id);
        var result = new List<string>();
        if (await category.AnyAsync(x => x.TitleFa == entity.TitleFa))
            result.Add(nameof(entity.TitleFa));
        if (await category.AnyAsync(x => x.TitleEn == entity.TitleEn))
            result.Add(nameof(entity.TitleEn));
        
        if (!result.Any())
            await base.Update(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };
    }

    public async Task<List<string>> AutoCompleteSearch(string input)
    {
        return await _brands
            .Where(x => x.TitleFa.Contains(input) || x.TitleEn.Contains(input))
            .Take(20)
            .Select(x => x.TitleFa + " " + x.TitleEn)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<List<long>> GetBrandIdsByList(List<string> brands)
    {
        return await _brands
            .Where(x => brands.Contains(x.TitleFa + " " + x.TitleEn))
            .Select(x => x.Id)
            .ToListAsync();
    }
}
