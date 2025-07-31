using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuestBooks.Models
{
    public class BookData
    {
        [Key]
        [Display(Name ="編號")]
        [StringLength(36)]  //GUID長度為36 (32個16進位的數字+4個dash)
        [HiddenInput] //隱藏輸入框，因為這個欄位是自動生成的，不需要用戶輸入
        public string BookID { get; set; } = null!; //使用GUID

        [Required]
        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Photo { get; set; }   //照片可有可無所以加上"?"

        public string PhotoType { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //1-3.主文資料表與回覆內容資料表之間具關聯，一則主文可以有很多則回覆內容(所以Rebook關聯型態為List)
        public virtual List<ReBook>? ReBooks { get; set; }



        public string ReBookID { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //1-3.主文資料表與回覆內容資料表之間具關聯

        public string BookID { get; set; } = null!;  //外鍵，指向主文資料表的BookID

        public Book? Book { get; set; }





    }
}
