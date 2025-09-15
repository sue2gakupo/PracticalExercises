using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel
{
    public class DigitalProductCreateVM
    {

        // === Product 基本資訊 ===
        [Required(ErrorMessage = "請輸入商品名稱")]
        [StringLength(100)]
        public string ProductName { get; set; }

        // 商品描述
        public string? Description { get; set; }

        // 類別 ID（必填，從資料庫 Category 下拉選擇）
        [Required(ErrorMessage = "請選擇商品分類")]
        public string CategoryID { get; set; }

        // 所屬系列（可選填）
        public string? SeriesID { get; set; }

        // 價格
        [Required(ErrorMessage = "請輸入商品價格")]
        [Range(0, double.MaxValue, ErrorMessage = "價格必須大於等於 0")]
        public decimal Price { get; set; }

        //

        // 檔案上傳
        [Required(ErrorMessage = "請上傳檔案")]
        public IFormFile UploadFile { get; set; }

        // 預覽圖上傳（可選填）
        public IFormFile? PreviewImage { get; set; }

        // 下載次數限制
        [Required(ErrorMessage = "請輸入下載限制次數")]
        public int DownloadLimit { get; set; }

        // 檔案有效天數（例如 30 天），可選填
        public int? ValidityDays { get; set; }

        // 授權型態 (0=免費使用, 1=單次授權, 2=商用授權 ... 自訂)
        public byte? LicenseType { get; set; }

        // 授權描述
        public string? LicenseDescription { get; set; }
    }
}
