using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.ProductVariants;
using System.Linq;

namespace ProEShop.Services.Services;

public class ProductVariantService : GenericService<ProductVariant>, IProductVariantService
{
    private readonly DbSet<ProductVariant> _productVariants;
    private readonly IMapper _mapper;
    private readonly ISellerService _sellerService;

    public ProductVariantService(IUnitOfWork uow, IMapper mapper, ISellerService sellerService)
        : base(uow)
    {
        _mapper = mapper;
        _sellerService = sellerService;
        _productVariants = uow.Set<ProductVariant>();
    }

    public async Task<List<ShowProductVariantViewModel>> GetProductVariants(long productId)
    {
        var sellerId = await _sellerService.GetSellerIdAsync();
        return await _mapper.ProjectTo<ShowProductVariantViewModel>(
            _productVariants.Where(x => x.ProductId == productId && x.SellerId == sellerId)
        ).ToListAsync();
    }

    public async Task<int> GetVariantCodeForCreateProductVariant()
    {
        var lastProductVariantCode = await _productVariants
            .OrderByDescending(x => x.Id)
            .Select(x => x.VariantCode)
            .FirstOrDefaultAsync();
        return lastProductVariantCode + 1;
    }

    public async Task<ShowProductVariantInCreateConsignmentViewModel?> GetProductVariantForCreateConsignment(int variantCode)
    {
        var sellerId = await _sellerService.GetSellerIdAsync();
            return await _mapper.ProjectTo<ShowProductVariantInCreateConsignmentViewModel>(
                _productVariants.Where(x=>x.SellerId == sellerId))
            .SingleOrDefaultAsync(x => x.VariantCode == variantCode);
    }

    public async Task<List<GetProductVariantInCreateConsignmentViewModel>> GetProductVariantForCreateConsignment(List<int> variantCodes)
    {
        var sellerId = await _sellerService.GetSellerIdAsync();
        return await _mapper.ProjectTo<GetProductVariantInCreateConsignmentViewModel>(
            _productVariants
                .Where(x=>x.SellerId == sellerId)
                .Where(x => variantCodes.Contains(x.VariantCode))
        ).ToListAsync();
    }

    public Task<List<ProductVariant>> GetProductVariantsToAddCount(List<long> Ids)
    {
        return _productVariants.Where(x=>Ids.Contains(x.Id))
            .ToListAsync();
    }
}
