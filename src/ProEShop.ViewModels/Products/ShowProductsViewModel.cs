﻿using System.ComponentModel.DataAnnotations;
using ProEShop.Common.Helpers;

namespace ProEShop.ViewModels.Products;

public class ShowProductsViewModel
{
    public List<ShowProductViewModel>? Products { get; set; }

    public SearchProductsViewModel SearchProducts { get; set; }
        = new();
    public PaginationViewModel Pagination { get; set; }
        = new();
}

public class ShowProductViewModel
{
    [Display(Name = "شناسه")]
    public long Id { get; set; }

    [Display(Name = "عنوان فارسی")]
    public string? PersianTitle { get; set; }
}

public class SearchProductsViewModel
{
    [ContainsSearch]
    [Display(Name = "عنوان فارسی")]
    [MaxLength(200)]
    public string? PersianTitle { get; set; }

    [Display(Name = "وضعیت حذف شده ها")]
    public DeletedStatus DeletedStatus { get; set; }

    [Display(Name = "نمایش بر اساس")]
    public SortingProducts Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }
}

public enum SortingProducts
{
    [Display(Name = "شناسه")]
    Id,

    [Display(Name = "عنوان")]
    PersianTitle
}