using Microsoft.AspNetCore.Http;
using System.Drawing.Imaging;
using System.Drawing;
using BarcodeLib;

namespace ProEShop.Common.Helpers;

public static class FileHelper
{
    public static bool IsFileUploaded(this IFormFile file)
        => file != null && file.Length > 0;
    public static string GenerateBarcode(string barcode, string productTitle, bool isVariantColor, string variantValue)
    {
        #region MainBitmap

        // Create bitmap file for draw
        var mainBitmap = new Bitmap(300, 131);
        //Draw a recrectangle on all over file
        using var rectangleGraphics = Graphics.FromImage(mainBitmap);
        {
            var rectangle = new Rectangle(0, 0, 300, 131);
            rectangleGraphics.FillRectangle(Brushes.OrangeRed, rectangle);
        }

        //Save into memory 
        using var rectangleStream = new MemoryStream();
        {
            mainBitmap.Save(rectangleStream, ImageFormat.Png);
        }
        //return Convert.ToBase64String(rectangleStream.ToArray());

        #endregion

        #region Barcode

        // BarcodeLib package
        // Create Barcode
        var barcodeInstance = new Barcode();
        var barcodeImage = barcodeInstance.Encode(BarcodeLib.TYPE.CODE39, barcode, Color.Black,
            Color.OrangeRed, 300, 50);
        //Save into memory
        using var barcodeStream = new MemoryStream();
        {
            barcodeImage.Save(barcodeStream, ImageFormat.Png);
        }

        //return Convert.ToBase64String(barcodeStream.ToArray());

        #endregion

        #region MergedRectangleAndBarcode

        //Read bitmap and barcode from memory
        var newMainBitmap = (Bitmap)Image.FromStream(rectangleStream);
        var newBarcodeBitmap = (Bitmap)Image.FromStream(barcodeStream);

        //draw in bracode on bitmap
        using var newRectangleGraphics = Graphics.FromImage(newMainBitmap);
        {
            newRectangleGraphics.DrawImage(newBarcodeBitmap, 0, 0);
        }
        using var mergedRectangleAndBarcodeStream = new MemoryStream();
        {
            newMainBitmap.Save(mergedRectangleAndBarcodeStream, ImageFormat.Png);
        }

        //return Convert.ToBase64String(mergedRectangleAndBarcodeStream.ToArray());

        #endregion

        #region WriteProductTitle

        var barcodeBitmap = (Bitmap)Image.FromStream(mergedRectangleAndBarcodeStream);
        using var graphics = Graphics.FromImage(barcodeBitmap);
        {
            using var font = new Font("Tahoma", 10);
            {
                //Create unvisiable rectangle for alignment and trimming
                var rect = new Rectangle(0, 55, 300, 52);
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                // Cut extra text and write multiple dots instead (...)
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                graphics.DrawString(productTitle, font, Brushes.Black, rect, sf);
                //graphics.DrawRectangle(Pens.Green, rect);
            }
        }

        using var productTitleStream = new MemoryStream();
        {
            barcodeBitmap.Save(productTitleStream, ImageFormat.Png);
        }
        //return Convert.ToBase64String(productTitleStream.ToArray());

        #endregion

        #region WriteVariant

        var variantText = isVariantColor ? "رنگ" : "اندازه";
        variantText += $": {variantValue}";

        var barcodeWithProductTitleBitmap = (Bitmap)Image.FromStream(productTitleStream);
        using var graphics2 = Graphics.FromImage(barcodeWithProductTitleBitmap);
        {
            using var font = new Font("Tahoma", 10);
            {
                var rect = new Rectangle(0, 112, 300, 19);
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                graphics2.DrawString(variantText, font, Brushes.Black, rect, sf);
                //graphics2.DrawRectangle(Pens.Green, rect);
            }
        }

        using var finalStream = new MemoryStream();
        {
            barcodeWithProductTitleBitmap.Save(finalStream, ImageFormat.Png);
        }

        return Convert.ToBase64String(finalStream.ToArray());
        #endregion
    }
}