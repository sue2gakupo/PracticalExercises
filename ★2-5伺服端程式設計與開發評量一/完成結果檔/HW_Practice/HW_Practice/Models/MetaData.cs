using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuestBooks.Models
{
    //1-5各類別中須設計相對應屬性資料型態(Data Type)及驗證規則。
    public class BookData
    {
        //2-2 使用者發表文章時須填寫主題、發表內容、發表人欄位，而照片可有可無。
        [Key]
        [Display(Name = "留言編號")]
        [StringLength(36)]
        [HiddenInput]
        public string BookID { get; set; } = null!;

        [Display(Name = "發表主題")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "主題長度需介於3~40個字")]
        [Required(ErrorMessage = "發表主題必填")]
        public string Title { get; set; } = null!;
        
        [Display(Name = "發表內容")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "發表內容必填")]
        public string Description { get; set; } = null!;

        [Display(Name = "照片")]
        [StringLength(41)] //36碼GUID+.JPG/.PNG/.JPEG(因為照片副檔名最長為5個字元)
        public string? Photo { get; set; }


        [Display(Name = "發表人名稱")]
        [StringLength(20, ErrorMessage = "發表人名稱最多20個字")]
        [Required(ErrorMessage = "發表人名稱必填")]

        public string Author { get; set; } = null!;

        [Display(Name = "發布時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd hh:mm:ss}")]
        [HiddenInput]
        public DateTime CreateDate { get; set; } = DateTime.Now;


    }


    public class ReBookData
    {
        [Key]
        [Display(Name = "回覆編號")]
        [StringLength(36, MinimumLength = 36)]
        [HiddenInput]
        public string ReBookID { get; set; } = null!;

        [Required(ErrorMessage = "回覆內容必填")]
        [Display(Name = "回覆內容")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = null!;


        [Required(ErrorMessage = "回覆人名稱必填")]
        [Display(Name = "回覆人名稱")]
        [StringLength(20, ErrorMessage = "回覆人名稱最多20個字")]
        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //1-3.主文資料表與回覆內容資料表之間具關聯

        [ForeignKey("Book")]
        [HiddenInput]
        public string BookID { get; set; } = null!;

    }

    [ModelMetadataType(typeof(BookData))]
    public partial class Book
    { 
    }
    [ModelMetadataType(typeof(ReBookData))]
    public partial class ReBook
    { 
    }



    }
