using Microsoft.EntityFrameworkCore;

namespace GuestBooks.Models
{
    //繼承GuestBookContext內容
    public class GuestBookContext : DbContext
    {
        //DbContext 是 Entity Framework Core 的核心類別，用來管理資料表、資料列、查詢、儲存等所有資料庫操作。
        //依賴注入的建構子
        public GuestBookContext(DbContextOptions<GuestBookContext> options)
            : base(options)
        { 
        }
        
        //描述資料庫的資料表
        public virtual DbSet<Book> Books { get; set; } //主文資料表
        public virtual DbSet<ReBook> ReBooks { get; set; } //回覆內容資料表        

    




    }
}

















