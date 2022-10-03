using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.Common.Helpers;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels;
using ProEShop.ViewModels.Products;
using ProEShop.ViewModels.Sellers;


namespace ProEShop.Services.Services;

public class ProductService : GenericService<Product>, IProductService
{
    private readonly DbSet<Product> _products;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _products = uow.Set<Product>();
    }


    public async Task<ShowProductsViewModel> GetProducts(ShowProductsViewModel model)
    {
        var products = _products.AsQueryable().AsNoTracking();

        #region Search

        products = ExpressionHelpers.CreateSearchExpressions(products, model.SearchProducts);

        #endregion

        #region OrderBy

        products = products.CreateOrderByExpression(model.SearchProducts.Sorting.ToString(),
            model.SearchProducts.SortingOrder.ToString());
        #endregion

        var paginationResult = await GenericPaginationAsync(products, model.Pagination);

        return new()
        {
            Products = await _mapper.ProjectTo<ShowProductViewModel>(paginationResult.Query)
                .ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }
}
