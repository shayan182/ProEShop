namespace ProEShop.Common.Helpers;

public static class StringHelpers
{
    public static bool IsEmail(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;
        return input.Contains("@");
    }

    public static string GenerateGuid() => Guid.NewGuid().ToString("N");
}