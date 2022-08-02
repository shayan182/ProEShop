using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEShop.Common.Helpers;
public class ConvertDateForCreateSeller
{
    public ConvertDateForCreateSeller(bool isOk, bool isGreaterThan18 = default, DateTime convetedDateTime = default)
    {
        IsOk = isOk;
        IsGreaterThan18 = isGreaterThan18;
        ConvetedDateTime = convetedDateTime;
    }

    public bool IsOk { get; set; }
    public bool IsGreaterThan18 { get; set; }
    public DateTime ConvetedDateTime { get; set; }

}
