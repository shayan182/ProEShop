using ProEShop.Common.Constants;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProEShop.ViewModels.Features;

public class ShowFeaturesViewModel
{
    public List<ShowFeatureViewModel> Features { get; set; }
    = new();
    public SearchFeaturesViewModel SearchFeatures { get; set; }
        = new();
    public PaginationViewModel Pagination { get; set; }
    = new();
}

public class ShowFeatureViewModel
{
    [Display(Name = "عنوان")]
    public string Title { get; set; }

    public long CategoryId { get; set; }
    public long FeatureId { get; set; }
}
public class SearchFeaturesViewModel
{
    [Display(Name = "دسته بندی")]
    public long CategoryId { get; set; }

    [Display(Name = "عنوان")]
    [MaxLength(150,ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Title { get; set; }  
    public List<SelectListItem>? Categories { get; set; }
    [Display(Name = "نمایش بر اساس")]
    public SortingFeatures Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }

}

public enum SortingFeatures
{
    [Display(Name = "شناسه")]
    Id,
    [Display(Name = "حذف شده ها")]
    IsDeleted
}

