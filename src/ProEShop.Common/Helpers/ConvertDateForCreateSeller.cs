using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEShop.Common.Helpers;
public class ConvertDateForCreateSeller
{
    public ConvertDateForCreateSeller(bool isOk, bool isRangeOk = default, DateTime convetedDateTime = default)
    {
        IsOk = isOk;
        IsRangeOk = isRangeOk;
        ConvetedDateTime = convetedDateTime;
    }

    public bool IsOk { get; set; }
    public bool IsRangeOk { get; set; }
    public DateTime ConvetedDateTime { get; set; }

}
