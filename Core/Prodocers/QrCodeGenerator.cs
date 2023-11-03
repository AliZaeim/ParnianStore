using DocumentFormat.OpenXml.ExtendedProperties;
using Net.Codecrete.QrCodeGenerator;
using QRCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace Core.Prodocers
{
    public static class QrCodeGenerator
    {
        public static byte[] GenerateByteArray(string url)
        {
            var image = GenerateImage(url);
            return ImageToByte(image);
        }

        public static Bitmap GenerateImage(string url)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q, true, true);
            QRCoder.QRCode qrCode = new(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(10);
            return qrCodeImage;
        }

        private static byte[] ImageToByte(Image img)
        {
            using var stream = new MemoryStream();
            img.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
        public static string GnCode(string QrText)
        {
            using (MemoryStream stream = new())
            {
                QRCodeGenerator qrGenerator = new();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrText, QRCodeGenerator.ECCLevel.Q, true, true, QRCodeGenerator.EciMode.Utf8);
                QRCoder.QRCode qrCode = new(qrCodeData);

                using Bitmap obitmap = qrCode.GetGraphic(20);
                obitmap.Save(stream, ImageFormat.Png);
                return "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
            }
        }
        public static string ZxingGenerateQrCode(string Text)
        {
            using (MemoryStream stream = new())
            {

                QrCodeEncodingOptions options = new()
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = 250,
                    Height = 250,
                    GS1Format = true
                };
                
                BarcodeWriter writer = new();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = options;
                
                BarcodeWriter qr = new();
                qr.Options = options;
                qr.Format = BarcodeFormat.QR_CODE;
                string DeText = DecodeUTF8String(Text);
                using Bitmap obitmap = new(qr.Write(DeText));
                obitmap.Save(stream, ImageFormat.Png);
                return "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
            }


        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
        private static string DecodeUTF8String(string utf8Str)

        {

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");

            Encoding utf8 = Encoding.UTF8;

            byte[] utfBytes = utf8.GetBytes(utf8Str);

            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);

            UTF8Encoding encoding = new UTF8Encoding();

            return encoding.GetString(isoBytes);

        }
    }
}
