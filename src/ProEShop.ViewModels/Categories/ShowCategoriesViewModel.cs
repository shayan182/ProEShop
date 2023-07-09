using ProEShop.Common.Constants;
using System.ComponentModel.DataAnnotations;
using ProEShop.ViewModels.Brands;

namespace ProEShop.ViewModels.Categories;

public class ShowCategoriesViewModel
{
    public List<ShowCategoryViewModel> Categories { get; set; }
    = new();
    public SearchCategoryViewModel SearchCategories { get; set; }
    = new();
    public PaginationViewModel Pagination { get; set; }
    = new();
}

public class ShowCategoryViewModel
{
    [Display(Name = "شناسه")]
    public long Id { get; set; }
    [Display(Name = "عنوان")]
    public string? Title { get; set; }

    [Display(Name = "والد")]
    public string? Parent { get; set; }

    [Display(Name = "آدرس")]
    public string? Slug { get; set; }

    [Display(Name = "نمایش در منو های اصلی")]
    public bool ShowInMenus { get; set; }
    public string? Picture { get; set; }
    public bool IsDeleted { get; set; }
    public bool ShowEditVariantButton { get; set; }

}
public class SearchCategoryViewModel
{
    [Display(Name = "عنوان")]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Title { get; set; }

    [Display(Name = "آدرس دسته بندی")]
    [MaxLength(130, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Slug { get; set; }

    [Display(Name = "نمایش در منو های اصلی")]
    public ShowInMenusStatus ShowInMenusStatus { get; set; }

    [Display(Name = "وضعیت حذف شده ها")]
    public DeletedStatus DeletedStatus { get; set; }
    [Display(Name = "نمایش بر اساس")]
    public SortingCategories Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }

}

public enum SortingCategories
{
    [Display(Name = "شناسه")]
    Id,

    [Display(Name = "عنوان")]
    Title,

    [Display(Name = "آدرس دسته بندی")]
    Slug,

    [Display(Name = "نمایش در منو های اصلی")]
    ShowInMenuStatus,

    [Display(Name = "حذف شده ها")]
    IsDeleted
}
public enum ShowInMenusStatus
{
    [Display(Name = "همه")]
    All,
    [Display(Name = "بله")]
    True,
    [Display(Name = "خیر")]
    False
}
