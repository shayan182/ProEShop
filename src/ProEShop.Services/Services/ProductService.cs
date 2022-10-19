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

        var searchedShopName = model.SearchProducts.ShopName;
        if (searchedShopName is not null)
            products = products.Where(x => x.Seller.ShopName.Contains(searchedShopName));

        var searchedStatus = model.SearchProducts.Status;
        if (searchedStatus is not null)
            products = products.Where(x => x.Status == searchedStatus);

        products = ExpressionHelpers.CreateSearchExpressions(products, model.SearchProducts);

        #endregion

        #region OrderBy

        var sorting = model.SearchProducts.Sorting;
        var isSortingAsc = model.SearchProducts.SortingOrder == SortingOrder.Asc;
        if (sorting == SortingProducts.ShopName)
        {
            if (isSortingAsc)
                products = products.OrderBy(x => x.Seller.ShopName);
            
            else
                products = products.OrderByDescending(x => x.Seller.ShopName);
            
        } else if (sorting == SortingProducts.BrandFa)
        {
            if (isSortingAsc)
                products = products.OrderBy(x => x.Brand.TitleFa);

            else
                products = products.OrderByDescending(x => x.Brand.TitleFa);
        } else if (sorting == SortingProducts.BrandEn)
        {
            if (isSortingAsc)
                products = products.OrderBy(x => x.Brand.TitleEn);

            else
                products = products.OrderByDescending(x => x.Brand.TitleEn);
        }
        else
        {
            products = products.CreateOrderByExpression(model.SearchProducts.Sorting.ToString(),
                model.SearchProducts.SortingOrder.ToString());
        }
        #endregion

        var paginationResult = await GenericPaginationAsync(products, model.Pagination);

        return new()
        {
            Products = await _mapper.ProjectTo<ShowProductViewModel>(paginationResult.Query)
                .ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public async Task<List<string?>> GetPersianTitlesForAutocomplete(string input)
    {
        return await _products.AsNoTracking()
            .Where(x => x.PersianTitle.Contains(input))
            .Take(20)
            .Select(x => x.PersianTitle)
            .ToListAsync();

    }

    public async Task<ProductDetailsViewModel?> GetProductDetails(long productId)
    {
        return await _mapper
            .ProjectTo<ProductDetailsViewModel>(
                _products
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.ProductFeatures)
                .ThenInclude(x => x.Feature))
            .SingleOrDefaultAsync(x=>x.Id == productId);
    }
    public async Task<Product?> GetProductToRemoveInManagingProduct(long id)
    {
        return await _products.Where(x => x.Status == ProductStatus.AwaitingInitialApproval)
            .AsNoTracking()
            .Include(x=>x.ProductMedia)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

}
