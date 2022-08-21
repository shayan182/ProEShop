﻿using AutoMapper;
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

        #region Search

        brands = ExpressionHelpers.CreateSearchExpressions(brands, model);

        //var searchedTitleFa = model.SearchBrands.TitleFa?.Trim();
        //if (!string.IsNullOrWhiteSpace(searchedTitleFa))
        //    brands = brands.Where(x => x.TitleFa.Contains(searchedTitleFa));


        //var searchedTitleEn = model.SearchBrands.TitleEn?.Trim();
        //if (!string.IsNullOrWhiteSpace(searchedTitleEn))
        //{
        //    brands = brands.Where(x => x.TitleEn.Contains(searchedTitleEn));
        //}

        //var searchedBrandLinkEnd = model.SearchBrands.BrandLinkEn?.Trim();
        //if (!string.IsNullOrWhiteSpace(searchedBrandLinkEnd))
        //    brands = brands.Where(x => x.BrandLinkEn.Contains(searchedBrandLinkEnd));


        //var searchedJudiciaryLink = model.SearchBrands.JudiciaryLink?.Trim();
        //if (!string.IsNullOrWhiteSpace(searchedJudiciaryLink))
        //    brands = brands.Where(x => x.JudiciaryLink.Contains(searchedJudiciaryLink));


        //var searchedIsIranianBrand = model.SearchBrands.IsIranianBrand;
        //if (searchedIsIranianBrand is not null)
        //    brands = brands.Where(x => x.IsIranianBrand == searchedIsIranianBrand.Value);


        //if (model.SearchBrands.DeletedStatus != DeletedStatus.True)
        //{
        //    var isOnlyDeleted = model.SearchBrands.DeletedStatus == DeletedStatus.OnlyDeleted;
        //    brands = brands.Where(x => x.IsDeleted == isOnlyDeleted);
        //}

        #endregion

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

    public async Task<List<string>> AutoCompleteSearch(string input)
    {
        return await _brands
            .Where(x => x.TitleFa.Contains(input) || x.TitleEn.Contains(input))
            .Select(x => x.TitleFa + " " + x.TitleEn)
            .ToListAsync();
    }
}