using System.ComponentModel.DataAnnotations;

namespace GuestBooks.Models
{
    public partial class Book //建立Metadata之後要在原本Model加入partial關鍵字
    {
        //1-1.主文資料表(編號(P.K)、主題、發表內容、照片、照片類型、發表人、張貼時間)

        public string BookID { get; set; } = null!; //使用GUID

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Photo { get; set; }   //照片可有可無所以加上"?"

        public string PhotoType { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //1-3.主文資料表與回覆內容資料表之間具關聯，一則主文可以有很多則回覆內容(所以Rebook關聯型態為List)
        public virtual List<ReBook>? ReBooks { get; set; } 




    }
}
