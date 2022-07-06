using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProEShop.ViewModels.Categories;

public class EditCategoryViewModel : AddCategoryViewModel
{
    [HiddenInput]
    public long Id { get; set; }

    [Display(Name = "تصویر انتخاب شده")]
    public string? SelectedPicture { get; set; }
}