using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel
{
    public class VisitorProductViewModel
    {
        // from Product
  
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int PurchaseCount { get; set; }

        // from ProductVariation 
        public string VariationID { get; set; }
        public string VariationName { get; set; }
        public int Stock { get; set; } //可選購數量

        //from ProductImage 
        public string ImageID { get; set; }
        public string ImageUrl { get; set; }

        public string ImageCaption { get; set; }
        public int SortOrder { get; set; }

    }
}
