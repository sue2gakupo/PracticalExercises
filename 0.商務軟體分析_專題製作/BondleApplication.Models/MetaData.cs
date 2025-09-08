using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BondleApplication.Models
{
    public partial class CategoryData
    {
        [ScaffoldColumn(false)] // 不要在 View 中產生這個欄位的輸入元素
        [StringLength(8)]
        [DisplayName("類別編號")]

        public string CategoryID { get; set; } = null!;

        [Required(ErrorMessage = "請輸入類別名稱")]
        [StringLength(50, ErrorMessage = "類別名稱不可超過50個字")]
        [DisplayName("類別名稱")]
        public string CategoryName { get; set; } = null!;

        [StringLength(200, ErrorMessage = "描述不可超過200個字")]
        [DisplayName("類別描述")]
        public string? Description { get; set; }

      
        [StringLength(8)]
        [DisplayName("主類別")]
        public string? ParentCategoryID { get; set; }

    }

    [ModelMetadataType(typeof(CategoryData))]  
    public partial class Category
    { 
    }



}
