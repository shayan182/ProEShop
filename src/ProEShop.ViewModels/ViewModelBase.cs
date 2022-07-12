﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProEShop.ViewModels;

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
    public byte Take { get; set; }
    public int PagesCount { get; set; }
    public int StartPage => CurrentPage - 3 < 1 ? 1 : CurrentPage - 2;
    public int EndPage => CurrentPage + 3 > PagesCount ? PagesCount : CurrentPage + 2;
}
public class PaginationResultViewModel<T>
{   
    public IQueryable<T> Query { get; set; }
    public PaginationViewModel Pagination { get; set; }
}