using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Convertors
{
    public static class ChangeString
    {
        public static string ToBreaf(this string Text, int Charachter_length = 10)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                string Result = "";
                int textLength = Text.Length;
                string Isolatedtext;
                
                if (textLength > Charachter_length)
                {
                    Isolatedtext = Text.Trim().Substring(0, Charachter_length);

                    string[] IsolatedtextArray = Isolatedtext.Split(' ');
                    List<string> IsolList = new List<string>();
                    IsolList.AddRange(IsolatedtextArray);
                    string[] AllTextArray = Text.Trim().Split(' ');
                    List<string> TextList = new List<string>();
                    TextList.AddRange(AllTextArray);
                    List<string> shares = TextList.Intersect(IsolList).ToList();
                    foreach (var item in shares)
                    {
                        Result += item + " ";
                    }
                    Text = Result;
                }
            }
            return Text;
        }
       public static string ToPersianMounth(this int mounth)
        {
           if(mounth ==1)
            {
                return "فروردین";
            }
           else if(mounth ==2)
            {
                return "اردیبهشت";
            }
           else if(mounth == 3)
            {
                return "خــرداد";
            }
           else if(mounth == 4)
            {
                return "تــیر";
            }
           else if(mounth == 5)
            {
                return "مــرداد";
            }
           else if(mounth == 6)
            {
                return "شهــریور";
            }
           else if(mounth ==7)
            {
                return "مـــهر";
            }
           else if(mounth == 8)
            {
                return "آبــان";
            }
            else if (mounth == 9)
            {
                return "آذر";
            }
            else if (mounth == 10)
            {
                return "دی";
            }
            else if (mounth == 11)
            {
                return "بهــمن";
            }
            else if (mounth == 12)
            {
                return "اســفند";
            }
            return "نامشخص";
        }
        public static string Get_Nth_Character(this string Text,int lenght=20)
        {
            if(Text.Length > lenght)
            {
                return Text.Substring(0, lenght);
            }
            else
            {
                return Text;
            }
        }
    }
}
