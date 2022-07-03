﻿using ProEShop.Entities;
using ProEShop.ViewModels.Categories;

namespace ProEShop.Services.Contracts;
public interface ICategoryService : IGenericService<Category>
{
    Task<ShowCategoriesViewModel> GetCategories(SearchCategoryViewModel model);
    Dictionary<long, string> GetCategoriesToShowInSelelctBox();
}
