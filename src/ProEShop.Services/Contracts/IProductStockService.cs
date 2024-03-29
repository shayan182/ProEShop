﻿using ProEShop.Entities;

namespace ProEShop.Services.Contracts;

public interface IProductStockService : IGenericService<ProductStock>
{

    Task<ProductStock?> GetByProductVariantIdAndConsignmentId(long productVariantId, long consignmentId);
    Task<Dictionary<long, int>> GetProductStocksForAddProductVariantsCount(long consignmentId);

}