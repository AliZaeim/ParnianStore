using Core.DTOs.General;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;

namespace Core.Utility
{
    public static class HttpHelper
    {
        #region PreparePOSTForm
        /// <summary>
        /// کار این متد این است که یک فرم ایجاد کرده و اطلاعات پرداخت را به درگاه بانک ارسال می کنده
        /// This method prepares an Html form which holds all data in hidden field in the addetion to form submitting script.
        /// </summary>
        /// <param name="url">The destination Url to which the post and redirection will occur, the Url can be in the same App or ouside the App.</param>
        /// <param name="data">A collection of data that will be posted to the destination Url.</param>
        /// <returns>Returns a string representation of the Posting form.</returns>
        public static String PreparePOSTForm(string url, NameValueCollection data)
        {
            //Set a name for the form
            string formID = "PostForm";

            //Build the form using the specified data to be posted.
            StringBuilder strForm = new ();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            foreach (string key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
            }
            strForm.Append("</form>");

            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new ();
            strScript.Append("<script language='javascript'>");
            strScript.Append("$(form).submit()");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");

            //Return the form and the script concatenated. (The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }
        public static String PostData(string ExURL, NameValueCollection nameValueCollection)
        {
            string html = "<html><head>";
            html += "</head><body onload='document.forms[0].submit()'>";
            html += string.Format("<form name='PostForm' method='POST' action='{0}'>", ExURL);
            foreach (string key in nameValueCollection.Keys)
            {
                html += string.Format("<input name='{0}' type='text' value='{1}'>", key, nameValueCollection[key]);
            }
            html += "</form></body></html>";
            return html;
        }
        
        #endregion
    }

}
