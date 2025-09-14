using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel.Creator
{
    public class ProductSeriesViewModel
    {
        // 系列名稱
        [Required(ErrorMessage = "請輸入系列名稱")]
        [StringLength(80)]
        public string SeriesName { get; set; }

        // 系列描述（可選填）
        public string? Description { get; set; }

        // 封面圖上傳（可選填）
        public IFormFile? CoverImage { get; set; }

        // 標籤（可選填，使用逗號分隔）
        [StringLength(200)]
        public string? Tags { get; set; }

        // 排序（可選填，預設 0）
        public int SortOrder { get; set; } = 0;

        // 是否公開（預設公開）
        public bool IsPublic { get; set; } = true;
    }
}
