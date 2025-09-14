using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondleApplication.Models.ViewModel.Creator
{
    public class ProductImagesViewModel
    {

        public int SortOrder { get; set; }

        [StringLength(100)]
        public string? ImageCaption { get; set; }

        public string? VariationID { get; set; } // 新增此屬性以修正 CS1061
        public IFormFile ImageFile { get; set; } // 若有圖片上傳需求，建議補上


    }
}
