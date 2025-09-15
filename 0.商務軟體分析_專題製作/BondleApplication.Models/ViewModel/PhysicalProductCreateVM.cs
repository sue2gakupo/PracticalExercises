using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel
{
    public class PhysicalProductCreateVM
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

        // 重量 (kg)，可選填
        [Range(0, 99999999.999, ErrorMessage = "重量(公克)錯誤")]
        public decimal? Weight { get; set; }

        // 長度 (cm)，可選填
        [Range(0, 99999999.99, ErrorMessage = "長度(公分)錯誤")]
        public decimal? Length { get; set; }

        // 寬度 (cm)，可選填
        [Range(0, 99999999.99, ErrorMessage = "寬度(公分)錯誤")]
        public decimal? Width { get; set; }

        // 高度 (cm)，可選填
        [Range(0, 99999999.99, ErrorMessage = "高度(公分)錯誤")]
        public decimal? Height { get; set; }

        // 預估出貨天數（必填）
        [Required(ErrorMessage = "請輸入出貨天數")]
        [Range(1, 365, ErrorMessage = "出貨天數需介於 1–365 天之間")]
        public int DeliveryDays { get; set; }

        // 運費類型（1:免運費 2:固定運費 3:依重量 4:依體積）
        [Required(ErrorMessage = "請選擇運費類型")]
        public byte ShippingFeeType { get; set; }

        // 固定運費（ShippingFeeType=2時使用）
        [Range(0, double.MaxValue, ErrorMessage = "固定運費不能小於 0")]
        public decimal? FixedShippingFee { get; set; }

        // 是否為易碎品
        public bool IsFragile { get; set; }

        // 包裝備註（例如：附防撞包材）
        [StringLength(200, ErrorMessage = "包裝備註最多 200 字")]
        public string? PackagingNote { get; set; }
    }
}
