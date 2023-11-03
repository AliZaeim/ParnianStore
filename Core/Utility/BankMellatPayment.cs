using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class BankMellatPayment
    {
        public static void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }
        public static string GetDate()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                   DateTime.Now.Day.ToString().PadLeft(2, '0');
        }
        public static string GetTime()
        {
            return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                   DateTime.Now.Second.ToString().PadLeft(2, '0');
        }
    }
}
