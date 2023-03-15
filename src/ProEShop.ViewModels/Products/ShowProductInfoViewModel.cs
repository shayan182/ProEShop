﻿namespace ProEShop.ViewModels.Products;
public class ShowProductInfoViewModel
{
    public int ProductCode { get; set; }
    public string? PersianTitle { get; set; }
    public string? EnglishTitle { get; set; }
    public string? CategoryTitle { get; set; }
    public string? Slug { get; set; }
    public string? BrandTitleFa { get; set; }
    public string? BrandLogoPicture { get; set; }
    public string? SellerShopName { get; set; }
    public string? SellerLogo { get; set; }
    public string? CategoryProductPageGuide { get; set; }
    public List<ProductMediaForProductInfoViewModel>? ProductMedia { get; set; }

    public List<ProductCategoryForProductInfoViewModel>? ProductCategories { get; set; }
}

public class ProductMediaForProductInfoViewModel
{
    public string? FileName { get; set; }

    public bool IsVideo { get; set; }
}

public class ProductCategoryForProductInfoViewModel
{
    public string? CategorySlug { get; set; }

    public string? CategoryTitle { get; set; }
}