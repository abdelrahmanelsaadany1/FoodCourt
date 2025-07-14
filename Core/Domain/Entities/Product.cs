using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product:BaseEntity
    {
    
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        #region ProductBrand
        public int BrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }

        #endregion
        #region ProductType
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        #endregion
    }
}
