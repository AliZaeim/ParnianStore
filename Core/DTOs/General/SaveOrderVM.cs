using System;

namespace Core.DTOs.General
{
    public class SaveOrderVM
    {
        public string OrderDedicated { get; set; }
        public string UserPassword { get; set; }
        public string BuyerFullName { get; set; }
        public bool IsSuccess { get; set; }
        public Guid OrderId { get; set; }
        public bool IsNewUser { get; set; }
    }
}
