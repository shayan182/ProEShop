namespace ProEShop.Common.GuardToolKit;

public static class GuardExt
{
    /// <summary>
    /// Check if the argument is null
    /// </summary>
    public static void CheckArgumentIsNull(this object o, string message)
    {
        if(o == null)
            throw new NullReferenceException(message);
    }
}
