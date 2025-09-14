using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel.Creator
{
    public class ProductVariationsViewModel
    {
        [ScaffoldColumn(false)]
        public string? VariationID { get; set; }

        // SKU（唯一識別碼）
        [Required(ErrorMessage = "請輸入自訂款式編號")]
        [StringLength(50)]
        public string SKU { get; set; }

        // 規格名稱
        [Required(ErrorMessage = "請輸入款式名稱")]
        [StringLength(100)]
        public string VariationName { get; set; }

        // 顏色（可選填）
        [StringLength(30)]
        public string? Color { get; set; }

        // 尺寸（可選填）
        [StringLength(20)]
        public string? Size { get; set; }

        // 材質（可選填）
        [StringLength(30)]
        public string? Material { get; set; }

        // 版本（可選填）
        [StringLength(20)]
        public string? Edition { get; set; }

        // 價格差異（相對於主商品價格）
        [Column(TypeName = "money")]
        public decimal? PriceDifference { get; set; }

        // 庫存數量
        [Required(ErrorMessage = "請輸入庫存數量")]
        public int Stock { get; set; }

        // 安全庫存（可選填）
        public int SafetyStock { get; set; }

        // 是否啟用
        public bool IsActive { get; set; } = true;

        // 排序
        public int SortOrder { get; set; }

        // 是否為預設規格
        public bool IsDefault { get; set; } = false;
    }

}
