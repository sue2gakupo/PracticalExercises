using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel.Creator
{
    public class ProductCreateViewModel
    {
        // === Product 基本資訊 ===
        [Required(ErrorMessage = "請輸入商品名稱")]
        [StringLength(100)]
        public string ProductName { get; set; }

        // 商品描述
        public string? Description { get; set; }

        // 商品類型 (0 = Digital, 1 = Physical)
        [Required(ErrorMessage = "請選擇商品類型")]
        public byte ProductType { get; set; }

        // 類別 ID（必填，從資料庫 Category 下拉選擇）
        [Required(ErrorMessage = "請選擇商品分類")]
        public string CategoryID { get; set; }

        // 所屬系列（可選填）
        public string? SeriesID { get; set; }

        // 價格
        [Required(ErrorMessage = "請輸入商品價格")]
        [Range(0, double.MaxValue, ErrorMessage = "價格必須大於等於 0")]
        public decimal Price { get; set; }

        // === 額外資訊 ===
        public DigitalProductViewModel? Digital { get; set; }
        public PhysicalProductViewModel? Physical { get; set; }

        // 商品規格（可有多個）
        public List<ProductVariationsViewModel> Variations { get; set; } = new();

        // 商品圖片（對應 Variation）
        public List<ProductImagesViewModel> Images { get; set; } = new();

        // 選擇系列（若要新建系列，可以用 ProductSeriesViewModel）
        public ProductSeriesViewModel? NewSeries { get; set; }

    }
}
