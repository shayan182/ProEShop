using Microsoft.AspNetCore.Mvc;
using ProEShop.ViewModels.Variants;

namespace ProEShop.ViewModels.CategoryVariants;
public class EditCategoryVariantViewModel
{
    [HiddenInput]
    public long CategoryId { get; set; }
    public List<ShowVariantInEditCategoryVariantViewModel>? Variants { get; set; }

    public List<long> SelectedVariants { get; set; }
        = new();
}