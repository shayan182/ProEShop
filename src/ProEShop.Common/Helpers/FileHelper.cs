using Microsoft.AspNetCore.Http;

namespace ProEShop.Common.Helpers;

public static class FileHelper
{
    public static bool IsFileUploaded(this IFormFile file)
        => file != null && file.Length > 0;
}