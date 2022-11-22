﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.Common.Helpers;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Guarantees;

namespace ProEShop.Services.Services;

public class GuaranteeService : GenericService<Guarantee>, IGuaranteeService
{
    private readonly DbSet<Guarantee> _guarantees;
    private readonly IMapper _mapper;

    public GuaranteeService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _guarantees = uow.Set<Guarantee>();
    }

    public async Task<ShowGuaranteesViewModel> GetGuarantees(ShowGuaranteesViewModel model)
    {
        var guarantees = _guarantees.AsNoTracking().AsQueryable();

        #region Search

        guarantees = ExpressionHelpers.CreateSearchExpressions(guarantees, model.SearchGuarantees, callDeletedStatusExpression: false);

        #endregion

        #region OrderBy

        guarantees = guarantees.CreateOrderByExpression(model.SearchGuarantees.Sorting.ToString(),
            model.SearchGuarantees.SortingOrder.ToString());

        #endregion

        var paginationResult = await GenericPaginationAsync(guarantees, model.Pagination);

        return new()
        {
            Guarantees = await _mapper.ProjectTo<ShowGuaranteeViewModel>(
                paginationResult.Query
            ).ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public async Task<EditGuaranteeViewMode?> GetForEdit(long id)
    {
        return await _mapper.ProjectTo<EditGuaranteeViewMode>(
            _guarantees
        ).SingleOrDefaultAsync(x => x.Id == id);
    }
}