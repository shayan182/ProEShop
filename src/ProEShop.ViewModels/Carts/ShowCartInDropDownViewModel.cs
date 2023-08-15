namespace ProEShop.ViewModels.Carts;
public class ShowCartInDropDownViewModel
{
    public string? ProductVariantProductPersianTitle { get; set; }

    public bool IsDiscountActive { get; set; }

    public int ProductVariantPrice { get; set; }

    public int? ProductVariantOffPrice { get; set; }

    public string? ProductVariantVariantColorCode { get; set; }

    public string? ProductVariantVariantValue { get; set; }

    public short Count { get; set; }

    public string? ProductPicture { get; set; }
}