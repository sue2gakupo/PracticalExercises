using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuestBooks.Models
{
    //1-5各類別中須設計相對應屬性資料型態(Data Type)及驗證規則。
    public class BookData
    {
        //2-2 使用者發表文章時須填寫主題、發表內容、發表人欄位，而照片可有可無。
        [Key]
        [Display(Name = "編號")]
        [StringLength(36)]  //GUID長度為36 (32個16進位的數字+4個dash)
        [HiddenInput] //隱藏輸入框，因為這個欄位是自動生成的，不需要用戶輸入
        public string BookID { get; set; } = null!; //使用GUID

        [Required(ErrorMessage ="主題必填")]
        [Display(Name = "主題")]
        [StringLength(40,MinimumLength =3,ErrorMessage ="主題長度需介於3~40個字")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage ="發表內容必填")]
        [Display(Name = "發表內容")]
        [DataType(DataType.MultilineText)] //在view中的HTML裡面會轉換成textarea
        public string Description { get; set; } = null!;

        [Display(Name ="照片")]
        [StringLength(40)] //32個16進位數字+4個dash"-"(GUID 總共36個字元) + .jpg(4個字元) = 40個字元
                           //★但是若需要變更圖片類型要怎麼寫?
        public string? Photo { get; set; }   //照片可有可無所以加上"?"

        public string PhotoType { get; set; } = null!;

        [Required(ErrorMessage ="發表人名稱必填")]
        [Display(Name = "發表人名稱")]
        [StringLength(20,ErrorMessage ="發表人名稱最多20個字")]
        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //1-3.主文資料表與回覆內容資料表之間具關聯，一則主文可以有很多則回覆內容(所以Rebook關聯型態為List)
        public virtual List<ReBook>? ReBooks { get; set; }


    }


    public class ReBookData
    {
        public string ReBookID { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //1-3.主文資料表與回覆內容資料表之間具關聯

        public string BookID { get; set; } = null!;  //外鍵，指向主文資料表的BookID

        public Book? Book { get; set; }

    }
}
