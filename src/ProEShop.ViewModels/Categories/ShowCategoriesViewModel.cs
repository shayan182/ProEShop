using ProEShop.Common.Constants;
using System.ComponentModel.DataAnnotations;

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
    public long Id { get; set; }
    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "والد")]
    public string Parent { get; set; }

    [Display(Name = "آدرس دسته بندی")]
    public string Slug { get; set; }

    [Display(Name = "نمایش در منو های اصلی")]
    public bool ShowInMenus { get; set; }
    public string Picture { get; set; }

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
