namespace ProEShop.Common.Helpers;

public class DuplicateColumns
{
    public bool Ok { get; set; }
    public List<string> Columns { get; set; }

    public DuplicateColumns(bool ok = true)
    {
        Ok = ok;
    }
}