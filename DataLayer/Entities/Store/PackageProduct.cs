using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Store
{
    public class PackageProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PkId { get; set; }
        #region Relations
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        [ForeignKey(nameof(PkId))]
        public Package Package { get; set; }
        #endregion
    }
}
