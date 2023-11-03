﻿using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.General
{
    public class HPackageComponentVM
    {
        public List<Package> Packages { get; set; }
        public string ReturnUrl { get; set; }
        public string Title { get; set; }
        public string TitleClass { get; set; }
        public string TitleLineClass { get; set; }
        public string Link { get; set; }
        public int TotalCount { get; set; }
        public string Image { get; set; }
        public string BgClass { get; set; }
    }
}
