using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.ProductVariants;
using System.Linq;
using AutoMapper.QueryableExtensions;
using DNTPersianUtils.Core;
using ProEShop.Common.Helpers;

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

    public async Task<EditProductVariantViewModel?> GetDataForEdit(long id)
    {
        var sellerId = await _sellerService.GetSellerIdAsync();
        return await _productVariants
            .Where(x=>x.SellerId == sellerId)
            .ProjectTo<EditProductVariantViewModel>(
                _mapper.ConfigurationProvider,
                parameters:new{now = DateTime.Now})
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AddEditDiscountViewModel?> GetDataForAddEditDiscount(long id)
    {
        var sellerId = await _sellerService.GetSellerIdAsync();
        var result = await _mapper.ProjectTo<AddEditDiscountViewModel>
                (_productVariants.Where(x => x.SellerId == sellerId))
            .SingleOrDefaultAsync(x => x.Id == id);
        if (result?.OffPercentage > 0)
        {
            var parsedStartDateTime = DateTime.Parse(result.StartDateTime);
            result.StartDateTimeEn = parsedStartDateTime.ToString("yyyy/MM/dd HH:mm");
            result.StartDateTime = parsedStartDateTime.ToShortPersianDateTime().ToPersianNumbers();

            var parsedEndDateTime = DateTime.Parse(result.EndDateTime);
            result.EndDateTimeEn = parsedEndDateTime.ToString("yyyy/MM/dd HH:mm");
            result.EndDateTime = parsedEndDateTime.ToShortPersianDateTime().ToPersianNumbers();
        }

        return result;
    }

    public async Task<ProductVariant?> GetForEdit(long id)
    {
        var sellerId = await _sellerService.GetSellerIdAsync();
        return await _productVariants
            .Where(x => x.SellerId == sellerId)
            .SingleOrDefaultAsync(x => x.Id == id);
    }
}
