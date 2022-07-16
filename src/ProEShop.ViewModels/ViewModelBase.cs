using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProEShop.ViewModels;

public enum PageCount
{
    [Display(Name = "10 سطر")]
    Ten,

    [Display(Name = "25 سطر")]
    TwentyFive,

    [Display(Name = "50 سطر")]
    Fifty,

    [Display(Name = "100 سطر")]
    Hundred
}
public enum SortingOrder
{
    [Display(Name = "صعودی")]
    Asc,

    [Display(Name = "نزولی")]
    Desc,
}
public static class ViewModelConstants
{
    public const string AntiForgeryToken = "__RequestVerificationToken";
}
public enum DeletedStatus
{
    [Display(Name = "نمایش داده نشوند")]
    False,
    [Display(Name = "نمایش داده شوند")]
    True,
    [Display(Name = "فقط حذف شده ها ")]
    OnlyDeleted
}

public class PaginationViewModel
{
    [HiddenInput]
    public int CurrentPage { get; set; } = 1;

    [HiddenInput]
    public PageCount PageCount { get; set; } = PageCount.Ten;
    public int PagesCount { get; set; }
    public int StartPage => CurrentPage - 3 < 1 ? 1 : CurrentPage - 2;
    public int EndPage => CurrentPage + 3 > PagesCount ? PagesCount : CurrentPage + 2;
}
public class PaginationResultViewModel<T>
{   
    public IQueryable<T> Query { get; set; }
    public PaginationViewModel Pagination { get; set; }
}