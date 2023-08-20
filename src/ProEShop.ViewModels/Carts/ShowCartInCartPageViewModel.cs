namespace ProEShop.ViewModels.Carts;
public class ShowCartInCartPageViewModel
{
    public string? ProductVariantProductPersianTitle { get; set; }
    public byte ProductVariantCount { get; set; }
    public string? ProductVariantGuaranteeFullTitle { get; set; }

    public string? ProductVariantSellerShopName { get; set; }

    public short ProductVariantMaxCountInCart { get; set; }
    public long ProductVariantId { get; set; }

    public bool IsDiscountActive { get; set; }

    public int ProductVariantPrice { get; set; }

    public int? ProductVariantOffPrice { get; set; }

    public string? ProductVariantVariantColorCode { get; set; }
    public bool? ProductVariantVariantIsColor { get; set; }

    public string? ProductVariantVariantValue { get; set; }

    public short Count { get; set; }

    public string? ProductPicture { get; set; }

    //Start Custom

    public int ProductVariantOffPercentage { get; set; }
    public DateTime ProductVariantStartDateTime { get; set; }
    public DateTime ProductVariantEndDateTime { get; set; }

    //End Custom

    public byte Score
    {
        get
        {
            var result = Math.Ceiling((double)ProductVariantPrice / 10000);
            if (result <= 1)
                return 1;
            if (result >= 150)
                return 150;
            return (byte)result;
        }
    }
}