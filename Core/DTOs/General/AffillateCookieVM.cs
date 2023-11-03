using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs.General
{
    public class AffillateCookieVM
    {
        public int UserId { get; set; }
        public string  CookieValue { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
