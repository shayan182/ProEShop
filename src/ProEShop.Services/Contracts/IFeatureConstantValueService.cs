﻿using ProEShop.Entities;
using ProEShop.ViewModels.FeatureConstantValues;

namespace ProEShop.Services.Contracts;

public interface IFeatureConstantValueService : IGenericService<FeatureConstantValue>
{
    Task<ShowFeatureConstantValuesViewModel> GetFeatureConstantValues(ShowFeatureConstantValuesViewModel model);
    Task<List<ShowCategoryFeatureConstantValueViewModel>> GetFeatureConstantValuesByCategoryId(long categoryId);
}