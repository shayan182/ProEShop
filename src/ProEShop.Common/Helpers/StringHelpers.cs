using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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


    //custome
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

    public static List<string> SetDuplicateColumnsErrors<T>(this List<string> duplicateColumns)
    {
        var result = new List<string>();
        foreach (var item in duplicateColumns)
        {
            var columnDisplayName = typeof(T).GetProperty(item)!
                .GetCustomAttribute<DisplayAttribute>()!.Name;
            result.Add($"این {columnDisplayName} قبلا در سیستم ثبت شده است");
        }
        return result;
    }
    public static string GenerateFileName(this IFormFile file)
    {
        return GenerateGuid() + Path.GetExtension(file.FileName);
    }
}