using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel.Creator
{
    public class PhysicalProductViewModel
    {

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
