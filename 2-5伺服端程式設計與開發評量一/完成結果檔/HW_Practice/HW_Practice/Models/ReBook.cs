namespace GuestBooks.Models
{
    public partial class ReBook
    {
        //1-2.回覆內容資料表(編號(P.K)、回覆內容、回覆人、回覆時間)
        public string ReBookID { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //1-3.主文資料表與回覆內容資料表之間具關聯

        public string BookID { get; set; } = null!;  //外鍵，指向主文資料表的BookID

        public Book? Book { get; set; } //關聯屬性，指向主文資料表的Book實體，關聯哪一個主留言


    }

}
