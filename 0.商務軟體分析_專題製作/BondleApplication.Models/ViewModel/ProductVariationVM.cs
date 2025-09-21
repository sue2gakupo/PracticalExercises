using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel
{
    public class ProductVariationVM
    {
        public string VariationName { get; set; }
        public int Stock { get; set; }
        public decimal? PriceDifference { get; set; }
        public IFormFile? ImageFile { get; set; } //資料庫沒有此欄位
    }
}
