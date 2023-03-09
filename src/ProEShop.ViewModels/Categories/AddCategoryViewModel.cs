using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProEShop.Common.Attributes;
using ProEShop.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ProEShop.ViewModels.Categories;

public class AddCategoryViewModel
{
    [PageRemote(PageName ="Index",PageHandler ="CheckForTitle"
        ,ErrorMessage =AttributesErrorMessages.RemoteMessage,
        HttpMethod = "POST",
        AdditionalFields =ViewModelConstants.AntiForgeryToken)]
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Title { get; set; }

    [Display(Name = "توضیحات")]
    public string? Description { get; set; }

    [Display(Name = "آدرس دسته بندی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(130, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Slug { get; set; }

    [Display(Name = "تصویر")]
    [MaxFileSize(2)]
    [IsImage ]
    public IFormFile? Picture { get; set; }

    [Display(Name = "والد")]
    public long? ParentId { get; set; }

    [Display(Name = "نمایش در منو اصلی")]
    public bool ShowInMenus { get; set; }

    [Display(Name = "آیا میتوان کالای غیر اصل وارد کرد؟")]
    public bool CanAddFakeProduct { get; set; }

    [Display(Name = "راهنمای صفحه محصول")]
    [MaxLength(1000, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? ProductPageGuide { get; set; }
    public List<SelectListItem>? MainCategories { get; set; }
}