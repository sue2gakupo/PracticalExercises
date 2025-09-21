using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel
{
    public class ProductCreateVM
    {
        // Product 共用
        public string ProductName { get; set; } = null!;

        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ProductType { get; set; } // 1=Physical, 2=Digital
        public string? CategoryID { get; set; }
        public string CreatorID { get; set; } = null!;

        // 上傳
        public IFormFile? MainImage { get; set; }

        // Physical
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public int? DeliveryDays { get; set; }
        public int? ShippingFeeType { get; set; }
        public decimal? FixedShippingFee { get; set; }
        public bool? IsFragile { get; set; }
        public string? PackagingNote { get; set; }

        // Digital
        public string? FileFormat { get; set; }
        public long? FileSize { get; set; }
        public IFormFile? FilePath { get; set; }
        public int? DownloadLimit { get; set; }
        public int? ValidityDays { get; set; }
        public byte? LicenseType { get; set; }
        public string? LicenseDescription { get; set; }

        // Variations（僅 Digital 用）
        public List<ProductVariationVM> Variations { get; set; } = new();
    }
}
