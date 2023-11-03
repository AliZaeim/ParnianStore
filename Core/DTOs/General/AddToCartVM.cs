using DataLayer.Entities.Store;
using System;

namespace Core.DTOs.General
{
    public class AddToCartVM
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public bool Added { get; set; }
        public Guid CartId { get; set; }
        public bool IsLack { get; set; }//اتمام موجودی کالا
        public int ProductId { get; set; }
        public bool Removed { get; set; }
        public string Type { get; set; } //نوع محصول

    }
}
