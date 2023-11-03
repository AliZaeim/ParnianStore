using Core.DTOs.Admin;
using Core.Prodocers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class Applications
    {
        public static bool IsValidNC(this string NC)
        {

            char[] chArray = NC.ToCharArray();
            int[] numArray = new int[chArray.Length];
            for (int i = 0; i < chArray.Length; i++)
            {
                numArray[i] = (int)char.GetNumericValue(chArray[i]);
            }
            int num2 = numArray[9];
            string[] strArray = { "0000000000", "1111111111", "22222222222", "33333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            if (strArray.Contains(NC))
            {
                return false;
            }
            else
            {
                int num3 = ((((((((numArray[0] * 10) + (numArray[1] * 9)) + (numArray[2] * 8)) + (numArray[3] * 7)) + (numArray[4] * 6)) + (numArray[5] * 5)) + (numArray[6] * 4)) + (numArray[7] * 3)) + (numArray[8] * 2);
                int num4 = num3 - ((num3 / 11) * 11);
                if ((((num4 == 0) && (num2 == num4)) || ((num4 == 1) && (num2 == 1))) || ((num4 > 1) && (num2 == Math.Abs((int)(num4 - 11)))))
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }


        }
        public static bool IsValidEmail(this string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        public static void GetNewDImage(int Width, int Height, Stream streamImg, string saveFilePath)
        {
            Bitmap sourceImage = new(streamImg);

            using Bitmap objBitmap = new(Width, Height);

            objBitmap.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);
            using Graphics objGraphics = Graphics.FromImage(objBitmap);
            // Set the graphic format for better result cropping   
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            objGraphics.DrawImage(sourceImage, 0, 0, Width, Height);

            // Save the file path, note we use png format to support png file   
            objBitmap.Save(saveFilePath);
        }
        public static string SaveImageWithNewDimension(this IFormFile image, int width, int height, string root, string name)
        {
            try
            {
                string ImageName = name + Path.GetExtension(image.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), root, ImageName);
                Stream zstream = image.OpenReadStream();
                Bitmap Simage = new Bitmap(zstream);
                using Bitmap objBitmap = new Bitmap(width, height);
                objBitmap.SetResolution(Simage.HorizontalResolution, Simage.VerticalResolution);
                using Graphics objGraphics = Graphics.FromImage(objBitmap);
                // Set the graphic format for better result cropping   
                objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                objGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;

                objGraphics.DrawImage(Simage, 0, 0, width, height);

                // Save the file path, note we use png format to support png file   
                objBitmap.Save(ImagePath);
                return ImageName;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<FileValidation> UploadedImageValidation(this IFormFile uploadImage, int validWidth, int validHeght, int validLength, string[] validExtensions)
        {
            string Messages = string.Empty;
            decimal vlen = decimal.Divide(validLength, 100);
            decimal Vlenght = vlen * 1024 * 1024;
            bool Valid = true;
            if (uploadImage == null)
            {
                Messages = "تصویری انتخاب نشده است !";
                Valid = false;
                return new FileValidation { IsValid = Valid, Message = Messages };
            }
            string Ex = Path.GetExtension(uploadImage.FileName);
            if (string.IsNullOrEmpty(Ex))
            {
                Valid = false;
                return new FileValidation { IsValid = false, Message = "پسوند فایل نامشخص است !" };
            }
            if (!validExtensions.Any(a => a == Ex))
            {
                Valid = false;
                if (string.IsNullOrEmpty(Messages))
                {
                    Messages = "پسوند فایل نامعتبر است !";
                }
                else
                {
                    Messages += Environment.NewLine + "پسوند فایل نامعتبر است !";
                }
            }
            if (uploadImage.Length > Vlenght)
            {
                Valid = false;
                if (string.IsNullOrEmpty(Messages))
                {
                    Messages = "حجم تصویر بزرگتر از " + validLength + " " + "کیلو بایت" + " می باشد !";
                }
                else
                {
                    Messages += Environment.NewLine + "حجم تصویر بزرگتر از " + validLength + " " + "کیلو بایت" + " می باشد !";
                }

            }
            using (var memoryStream = new MemoryStream())
            {
                await uploadImage.CopyToAsync(memoryStream);
                if (validLength != 0 && validWidth != 0)
                {
                    using (var img = Image.FromStream(memoryStream))
                    {
                        // TODO: ResizeImage(img, 100, 100);
                        if (img.Width != validWidth)
                        {
                            Valid = false;
                            if (string.IsNullOrEmpty(Messages))
                            {
                                Messages = "عرض تصویر باید " + validWidth + " پیکسل باشد !";
                            }
                            else
                            {
                                Messages += Environment.NewLine + "عرض تصویر باید " + validWidth + " پیکسل باشد !";
                            }

                        }
                        if (img.Height != validHeght)
                        {
                            Valid = false;
                            if (string.IsNullOrEmpty(Messages))
                            {
                                Messages = "ارتفاع تصویر باید " + validHeght + " پیکسل باشد !";
                            }
                            else
                            {
                                Messages += Environment.NewLine + "ارتفاع تصویر باید " + validHeght + " پیکسل باشد !";
                            }

                        }
                    }
                }

            }

            return new FileValidation { IsValid = Valid, Message = Messages };


        }
        public static async Task<FileValidation> UploadedImageValidation(this IFormFile uploadImage, int validLength, string[] validExtensions)
        {
            string Messages = string.Empty;
            decimal vlen = decimal.Divide(validLength, 100);
            decimal Vlenght = vlen * 1024 * 1024;
            bool Valid = true;
            if (uploadImage == null)
            {
                Messages = "تصویری انتخاب نشده است !";
                Valid = false;
                return new FileValidation { IsValid = Valid, Message = Messages };
            }
            string Ex = Path.GetExtension(uploadImage.FileName);
            if (string.IsNullOrEmpty(Ex))
            {
                Valid = false;
                return new FileValidation { IsValid = false, Message = "پسوند فایل نامشخص است !" };
            }
            if (!validExtensions.Any(a => a == Ex))
            {
                Valid = false;
                if (string.IsNullOrEmpty(Messages))
                {
                    Messages = "پسوند فایل نامعتبر است !";
                }
                else
                {
                    Messages += Environment.NewLine + "پسوند فایل نامعتبر است !";
                }
            }
            if (uploadImage.Length > Vlenght)
            {
                Valid = false;
                if (string.IsNullOrEmpty(Messages))
                {
                    Messages = "حجم تصویر بزرگتر از " + validLength + " " + "کیلو بایت" + " می باشد !";
                }
                else
                {
                    Messages += Environment.NewLine + "حجم تصویر بزرگتر از " + validLength + " " + "کیلو بایت" + " می باشد !";
                }

            }
            using (var memoryStream = new MemoryStream())
            {
                await uploadImage.CopyToAsync(memoryStream);


            }

            return new FileValidation { IsValid = Valid, Message = Messages };


        }
        public static string SaveUploadedImage(this IFormFile uploadImage, string root, bool SetFileName)
        {
            try
            {
                string ImageName = uploadImage.FileName;
                if (SetFileName)
                {
                    ImageName = uploadImage.FileName;
                }
                else
                {
                    ImageName = Path.Combine(Generators.GenerateUniqueCode(), Path.GetExtension(uploadImage.FileName));
                }
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), root, ImageName);
                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }
                return ImageName;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static bool IsImage(this string source)
        {
            return (source.EndsWith(".png") || source.EndsWith(".jpg")
                || source.EndsWith(".jpeg") || source.EndsWith(".gif")
                || source.EndsWith(".png") || source.EndsWith(".bmp")
                || source.EndsWith(".tif") || source.EndsWith(".tiff")
                || source.EndsWith(".ico")
                || source.EndsWith(".webp") || source.EndsWith(".avif")
                );
        }
        public static string GetLetterOfText(this string Text, int count)
        {
            int txtL = Text.Length;
            if (count > txtL)
            {
                return Text;
            }
            return Text.Substring(0, count);
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static Guid ToGuid(long value)
        {
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(value), guidData, 8);
            return new Guid(guidData);
        }
        public static long ToLong(Guid guid)
        {
            if (BitConverter.ToInt64(guid.ToByteArray(), 8) != 0)
                throw new OverflowException("Value was either too large or too small for an Int64.");
            return BitConverter.ToInt64(guid.ToByteArray(), 0);
        }


        private static readonly HashSet<char> DefaultNonWordCharacters
          = new HashSet<char> { ',', '.', ':', ';' };

        /// <summary>
        /// Returns a substring from the start of <paramref name="value"/> no 
        /// longer than <paramref name="length"/>.
        /// Returning only whole words is favored over returning a string that 
        /// is exactly <paramref name="length"/> long. 
        /// </summary>
        /// <param name="value">The original string from which the substring 
        /// will be returned.</param>
        /// <param name="length">The maximum length of the substring.</param>
        /// <param name="nonWordCharacters">Characters that, while not whitespace, 
        /// are not considered part of words and therefor can be removed from a 
        /// word in the end of the returned value. 
        /// Defaults to ",", ".", ":" and ";" if null.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="length"/> is negative
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="value"/> is null
        /// </exception>
        public static string CropWholeWords(
          this string value,
          int length,
          HashSet<char> nonWordCharacters = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (length < 0)
            {
                throw new ArgumentException("Negative values not allowed.", "length");
            }

            if (nonWordCharacters == null)
            {
                nonWordCharacters = DefaultNonWordCharacters;
            }

            if (length >= value.Length)
            {
                return value;
            }
            int end = length;

            for (int i = end; i > 0; i--)
            {
                if (value[i].IsWhitespace())
                {
                    break;
                }

                if (nonWordCharacters.Contains(value[i])
                    && (value.Length == i + 1 || value[i + 1] == ' '))
                {
                    //Removing a character that isn't whitespace but not part 
                    //of the word either (ie ".") given that the character is 
                    //followed by whitespace or the end of the string makes it
                    //possible to include the word, so we do that.
                    break;
                }
                end--;
            }

            if (end == 0)
            {
                //If the first word is longer than the length we favor 
                //returning it as cropped over returning nothing at all.
                end = length;
            }

            return value.Substring(0, end);
        }

        private static bool IsWhitespace(this char character)
        {
            return character == ' ' || character == 'n' || character == 't';
        }
        public static string Modify_Product_Name(this string pName)
        {
            pName = CultureInfo.CurrentCulture.TextInfo.ToLower(pName).Trim();
            //Betelnut  Tea
            switch (pName)
            {
                case "anison":
                    {
                        pName = "Anise";
                        break;
                    }
                
                default:
                    break;
            }
            return pName;
        }
        public static class ConvertArabicNumberToEnglish
        {
            public static string toEnglishNumber(string input)
            {
                string EnglishNumbers = "";
                for (int i = 0; i < input.Length; i++)
                {
                    if (char.IsDigit(input[i]))
                    {
                        EnglishNumbers += char.GetNumericValue(input, i);
                    }
                    else
                    {
                        EnglishNumbers += input[i].ToString();
                    }
                }
                return EnglishNumbers;
            }
        }
       

    }
}
