using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Core.Utility
{
   

    public static class CookieExtensions
    {

        private static IHttpContextAccessor httpContextAccessor;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }
        public static bool ExistCookie(string cookieName)
        {
            bool ex = httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(cookieName);
            return ex;
        }
        
        public static void SetCookie(string key, string value, DateTime? expireTime)
        {
            CookieOptions option = new();

            if (expireTime.HasValue)
                option.Expires = expireTime.Value;
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
        public static void SetListToCookie<T>(string key, List<T> value, DateTime? expireTime)
        {
            CookieOptions option = new();
            
            string output = JsonConvert.SerializeObject(value);
            if (expireTime.HasValue)
                option.Expires = expireTime.Value;
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            
            httpContextAccessor.HttpContext.Response.Cookies.Append(key, output, option);
        }
        public static string ReadCookie(string key)
        {
            //read cookie from IHttpContextAccessor  
            string cookieValue = null;
            if (httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
            {
                cookieValue = httpContextAccessor.HttpContext.Request.Cookies[key];
            }
             
            return cookieValue;
        }
       
        public static List<T> ReadListFromCookie<T>(string key)
        {
            //read cookie from IHttpContextAccessor  
            string cookieValue = httpContextAccessor.HttpContext.Request.Cookies[key];
            if(!string.IsNullOrEmpty(cookieValue))
            {
                List<T> deserialized = JsonConvert.DeserializeObject<List<T>>(cookieValue);
                return deserialized;
            }
            else
            {
                return null;
            }
            
        }
        public static bool RemoveCookie(string key)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
            bool res = true;
            if(ExistCookie(key))
            {
                res = false;
            }
            return res;
        }
    }
}
