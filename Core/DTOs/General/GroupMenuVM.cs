using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs.General
{
    public class GroupMenuVM
    {
        public List<ProductGroup> ProductGroups { get; set; }
        [Display(Name ="موبایلی یا لپ تاپی")]
        public string Type { get; set; }
    }
}
