using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.General
{
    public class TrackingOrderVM
    {
        public bool ExistOrder { get; set; }
        public string Message { get; set; }
        public Order Order { get; set; }
    }
}
