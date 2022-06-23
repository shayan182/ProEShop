using System.Text;

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

    public static string RemoveMultipleSpaces(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return String.Empty;
        var ch = str.ToCharArray();

        for (int i = 0; i < ch.Length - 1; i++)
        {
            if (ch[i] == ' ')
            {
                if (ch[i + 1] == ' ')
                {
                    var aString = new string(ch);
                    aString = aString.Remove(i, 1);
                    ch = aString.ToCharArray();
                    i--;
                }
            }
        }

        return new string(ch).Trim();
    }
}