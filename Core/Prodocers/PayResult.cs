using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Prodocers
{
    public class PayResult
    {
        #region نمایش پیغام های نتیجه پرداخت بانک ملت

        /// <summary>
        /// این متد یک ورودی گرفته و نتیجه پیغام را بر می گرداند
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static string Mellat(string resultId)
        {
            string result = "";
            switch (resultId)
            {
                case "-100":
                    result = "پرداخت کنسل شده";
                    break;
                case "0":
                    result = "تراكنش با موفقيت انجام شد";
                    break;

                case "11":
                    result = "شماره كارت نامعتبر است ";
                    break;
                case "12":
                    result = "موجودي كافي نيست ";
                    break;
                case "13":
                    result = "رمز نادرست است ";
                    break;
                case "14":
                    result = "تعداد دفعات وارد كردن رمز بيش از حد مجاز است ";
                    break;
                case "15":
                    result = "كارت نامعتبر است ";
                    break;
                case "16":
                    result = "دفعات برداشت وجه بيش از حد مجاز است ";
                    break;
                case "17":
                    result = "كاربر از انجام تراكنش منصرف شده است ";
                    break;
                case "18":
                    result = "تاريخ انقضاي كارت گذشته است ";
                    break;
                case "19":
                    result = "مبلغ برداشت وجه بيش از حد مجاز است ";
                    break;
                case "111":
                    result = "صادر كننده كارت نامعتبر است ";
                    break;
                case "112":
                    result = "خطاي سوييچ صادر كننده كارت ";
                    break;
                case "113":
                    result = "پاسخي از صادر كننده كارت دريافت نشد ";
                    break;
                case "114":
                    result = "دارنده كارت مجاز به انجام اين تراكنش نيست";
                    break;
                case "21":
                    result = "پذيرنده نامعتبر است ";
                    break;
                case "23":
                    result = "خطاي امنيتي رخ داده است ";
                    break;
                case "24":
                    result = "اطلاعات كاربري پذيرنده نامعتبر است ";
                    break;
                case "25":
                    result = "مبلغ نامعتبر است ";
                    break;
                case "31":
                    result = "پاسخ نامعتبر است ";
                    break;

                case "32":
                    result = "فرمت اطلاعات وارد شده صحيح نمي باشد ";
                    break;
                case "33":
                    result = "حساب نامعتبر است ";
                    break;
                case "34":
                    result = "خطاي سيستمي ";
                    break;
                case "35":
                    result = "تاريخ نامعتبر است ";
                    break;
                case "41":
                    result = "شماره درخواست تكراري است ، دوباره تلاش کنید";
                    break;
                case "42":
                    result = "يافت نشد  Sale تراكنش";
                    break;
                case "43":
                    result = "داده شده است  Verify قبلا درخواست";
                    break;
                case "44":
                    result = "يافت نشد  Verfiy درخواست";
                    break;
                case "45":
                    result = "شده است  Settle تراكنش";
                    break;
                case "46":
                    result = "نشده است  Settle تراكنش";
                    break;
                case "47":
                    result = "يافت نشد  Settle تراكنش";
                    break;
                case "48":
                    result = "شده است  Reverse تراكنش";
                    break;
                case "49":
                    result = "يافت نشد  Refund تراكنش";
                    break;
                case "412":
                    result = "شناسه قبض نادرست است ";
                    break;
                case "413":
                    result = "شناسه پرداخت نادرست است ";
                    break;
                case "414":
                    result = "سازمان صادر كننده قبض نامعتبر است ";
                    break;
                case "415":
                    result = "زمان جلسه كاري به پايان رسيده است ";
                    break;
                case "416":
                    result = "خطا در ثبت اطلاعات ";
                    break;
                case "417":
                    result = "شناسه پرداخت كننده نامعتبر است ";
                    break;

                case "418":
                    result = "اشكال در تعريف اطلاعات مشتري ";
                    break;
                case "419":
                    result = "تعداد دفعات ورود اطلاعات از حد مجاز گذشته است ";
                    break;
                case "421":
                    result = "نامعتبر است  IP";
                    break;
                case "51":
                    result = "تراكنش تكراري است ";
                    break;
                case "54":
                    result = "تراكنش مرجع موجود نيست ";
                    break;
                case "55":
                    result = "تراكنش نامعتبر است ";
                    break;
                case "61":
                    result = "خطا در واريز ";
                    break;

                default:
                    result = string.Empty;
                    break;

            }
            return result;
        }

        #endregion       
    }
}
