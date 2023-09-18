using ProEShop.Entities.Enums;

namespace ProEShop.ViewModels.Carts;

public class PaymentViewModel
{
    public List<ShowCartInPaymentPageViewModel> CartItems { get; set; }
}
public class ShowCartInPaymentPageViewModel
{

    public bool IsDiscountActive { get; set; }

    public int ProductVariantPrice { get; set; }

    public int? ProductVariantOffPrice { get; set; }

    public string? ProductVariantVariantColorCode { get; set; }
    public bool? ProductVariantVariantIsColor { get; set; }

    public string? ProductVariantVariantValue { get; set; }

    public short Count { get; set; }

    public string? ProductPicture { get; set; } 
    public Dimension ProductVariantProductDimension { get; set; }

    //Start Custom

    //public int ProductVariantOffPercentage { get; set; }
    //public DateTime ProductVariantStartDateTime { get; set; }
    //public DateTime ProductVariantEndDateTime { get; set; }

    //End Custom

    public byte Score
    {
        get
        {
            var result = Math.Ceiling((ProductVariantPrice * Count) / (double)10000);
            if (result <= 1)
                return 1;
            if (result >= 150)
                return 150;
            return (byte)result;
        }
    }
}
