using System.ComponentModel.DataAnnotations;
using ProEShop.Common.Constants;

namespace ProEShop.ViewModels.Product;

public class AddProductViewModel
{
    [Display(Name = "برند محصول")]
    [Range(1,long.MaxValue,ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long BrandId { get; set; }

    [Display(Name = "اصالت کالا")]
    public bool IsFake { get; set; }
}