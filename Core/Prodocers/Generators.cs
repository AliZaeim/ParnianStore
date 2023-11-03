using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Core.Prodocers
{
    public static class Generators
    {
        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "123456789";
        const string SPECIALS = @"!@£$%^&*()#€";
        public static string GenerateUniqueCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GenerateUniqueString(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial, int stringSize)
        {
            char[] _password = new char[stringSize];
            string charSet = ""; // Initialise to blank
            Random _random = new Random();
            int counter;

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < stringSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
        public static string GenerateUniqueString(int lowercase, int uppercase, int numerics, int specials)
        {  
            Random random = new Random();
            string generated = "";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    LOWER_CASE[random.Next(LOWER_CASE.Length - 1)].ToString()
                );
            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    UPPER_CAES[random.Next(UPPER_CAES.Length - 1)].ToString()
                );
            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    NUMBERS[random.Next(NUMBERS.Length - 1)].ToString()
                );
            for (int i = 1; i <= specials; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    SPECIALS[random.Next(SPECIALS.Length - 1)].ToString()
                );
            return generated;

        }
        public static string GenerateUniqueString(List<string> Codes, int lowercase, int uppercase, int numerics, int specials)
        {           
            string generated = GenerateUniqueString(lowercase,uppercase,numerics,specials);            
            while(Codes.Any(x => x == generated))
            {
                generated = GenerateUniqueString(lowercase, uppercase, numerics, specials);
            }
            return generated;
        }
        public static string GenerateKey(List<string> Keys, int Length = 4)
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, Length);
            while (Keys.Any(a => a == key))
            {
                key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, Length);
            }
            return key;
        }
        public static string GenerateRandomColor()
        {
            Random random = new Random();
            string color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
            return color;
        }
    }
}

