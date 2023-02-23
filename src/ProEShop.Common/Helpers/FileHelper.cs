using Microsoft.AspNetCore.Http;
using System.Drawing.Imaging;
using System.Drawing;
using BarcodeLib;

namespace ProEShop.Common.Helpers;

public static class FileHelper
{
    public static bool IsFileUploaded(this IFormFile file)
        => file != null && file.Length > 0;
    public static string GenerateConsignmentItemBarcode(string barcode, string productTitle, bool isVariantColor, string variantValue)
    {
        var resultWidth = 250;
        var resultHeight = productTitle.Length > 70 ? 131 : 113;
        var barcodeHeight = 50;
        var productTitleHeight = resultHeight == 131 ? 52 : 34;
        var productTitleY = barcodeHeight + 5;
        var variantY = productTitleY + productTitleHeight + 5;
        #region MainBitmap

        // Create bitmap file for draw
        var mainBitmap = new Bitmap(resultWidth, resultHeight);
        //Draw a recrectangle on all over file
        using var rectangleGraphics = Graphics.FromImage(mainBitmap);
        {
            var rectangle = new Rectangle(0, 0, resultWidth, 131);
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
        //var barcodeInstance = new Barcode();
        //var barcodeImage = barcodeInstance.Encode(BarcodeLib.TYPE.CODE39, barcode, Color.Black,
        //    Color.OrangeRed, resultWidth, barcodeHeight);
        //using var barcodeStream = new MemoryStream();
        //{
        //    barcodeImage.Save(barcodeStream, ImageFormat.Png);
        //}

        //return Convert.ToBase64String(barcodeStream.ToArray());

        var barcodeImage = GenerateBarcodeImage(barcode, resultWidth, barcodeHeight);
        #endregion

        #region MergedRectangleAndBarcode

        //Read bitmap and barcode from memory
        var newMainBitmap = (Bitmap)Image.FromStream(rectangleStream);
        var newBarcodeBitmap = barcodeImage;

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
                var rect = new Rectangle(0, productTitleY, resultWidth, productTitleHeight);
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                // Cut extra text and write multiple dots instead (...)
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                sf.LineAlignment = StringAlignment.Center;
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
        variantText += $" (کد تنوع:{barcode.Split("--")[0]})";

        var barcodeWithProductTitleBitmap = (Bitmap)Image.FromStream(productTitleStream);
        using var graphics2 = Graphics.FromImage(barcodeWithProductTitleBitmap);
        {
            using var font = new Font("Tahoma", 10);
            {
                var rect = new Rectangle(0, variantY, resultWidth, 19);
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

    public static string GenerateConsignmentItemBarcode(string barcode, string sellerShopName, string DeliveryDate)
    {
        var resultWidth = 250;
        var resultHeight = 98;
        var barcodeHeight = 50;
        var shopNameY = barcodeHeight + 5;
        var deliveryDateY = shopNameY + 19 + 5;
        #region MainBitmap

        // Create bitmap file for draw
        var mainBitmap = new Bitmap(resultWidth, resultHeight);
        //Draw a recrectangle on all over file
        using var rectangleGraphics = Graphics.FromImage(mainBitmap);
        {
            var rectangle = new Rectangle(0, 0, resultWidth, 131);
            rectangleGraphics.FillRectangle(Brushes.OrangeRed, rectangle);
        }

        //Save into memory 
        using var rectangleStream = new MemoryStream();
        {
            mainBitmap.Save(rectangleStream, ImageFormat.Png);
        }

        #endregion

        #region Barcode
        var barcodeImage = GenerateBarcodeImage(barcode, resultWidth, barcodeHeight);
        #endregion

        #region MergedRectangleAndBarcode

        //Read bitmap and barcode from memory
        var newMainBitmap = (Bitmap)Image.FromStream(rectangleStream);
        var newBarcodeBitmap = barcodeImage;

        //draw in bracode on bitmap
        using var newRectangleGraphics = Graphics.FromImage(newMainBitmap);
        {
            newRectangleGraphics.DrawImage(newBarcodeBitmap, 0, 0);
        }
        using var mergedRectangleAndBarcodeStream = new MemoryStream();
        {
            newMainBitmap.Save(mergedRectangleAndBarcodeStream, ImageFormat.Png);
        }


        #endregion

        #region WriteShopName

        var barcodeBitmap = (Bitmap)Image.FromStream(mergedRectangleAndBarcodeStream);
        using var graphics = Graphics.FromImage(barcodeBitmap);
        {
            using var font = new Font("Tahoma", 10);
            {
                //Create unvisiable rectangle for alignment and trimming
                var rect = new Rectangle(0, shopNameY, resultWidth, 19);
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                // Cut extra text and write multiple dots instead (...)
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                sf.LineAlignment = StringAlignment.Center;
                graphics.DrawString(sellerShopName, font, Brushes.Black, rect, sf);
                //graphics.DrawRectangle(Pens.Green, rect);
            }
        }

        using var productTitleStream = new MemoryStream();
        {
            barcodeBitmap.Save(productTitleStream, ImageFormat.Png);
        }

        #endregion

        #region WriteDeliveryDate

        var barcodeWithProductTitleBitmap = (Bitmap)Image.FromStream(productTitleStream);
        using var graphics2 = Graphics.FromImage(barcodeWithProductTitleBitmap);
        {
            using var font = new Font("Tahoma", 10);
            {
                var rect = new Rectangle(0, deliveryDateY, resultWidth, 19);
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                graphics2.DrawString(DeliveryDate, font, Brushes.Black, rect, sf);
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

    private static Bitmap GenerateBarcodeImage(string input, int width, int height)
    {
        // BarcodeLib package
        var barcodeInstance = new Barcode();
        var barcodeImage = barcodeInstance.Encode(BarcodeLib.TYPE.CODE39, input, Color.Black,
            Color.OrangeRed, width, height);
        using var barcodeStream = new MemoryStream();
        {
            barcodeImage.Save(barcodeStream, ImageFormat.Png);
        }
        return (Bitmap)Image.FromStream(barcodeStream);
    }
}