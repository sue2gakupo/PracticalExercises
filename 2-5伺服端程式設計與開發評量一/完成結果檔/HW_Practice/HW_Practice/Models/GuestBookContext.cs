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
        public virtual DbSet<Book> Book { get; set; } //主文資料表
        public virtual DbSet<ReBook> ReBook { get; set; } //回覆內容資料表        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //在DbContext中使用Fluent API在GuestBookContext覆寫 OnModelCreating 方法

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookID).HasName("PK_BookID");
                entity.Property(e => e.BookID).HasMaxLength(36).IsUnicode(false);

                entity.Property(e => e.Title)
                .HasMaxLength(40);

                entity.Property(e => e.Photo)
                .HasMaxLength(41);

                entity.Property(e => e.Author)
                .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                .HasColumnName("datatime");
            });

            modelBuilder.Entity<ReBook>(entity =>
            {
                entity.HasKey(e => e.ReBookID)
                .HasName("PK_ReBookID");
                entity.Property(e => e.ReBookID)
                .HasMaxLength(36).IsUnicode(false);

                entity.Property(e => e.Author)
                .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                .HasColumnName("datatime");

            });


        }
    }
}


















