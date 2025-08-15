using Microsoft.EntityFrameworkCore;

namespace GuestBooks.Models
{
    public class GuestBookContext : DbContext
    {
        public GuestBookContext(DbContextOptions<GuestBookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Book { get; set; } 
        public virtual DbSet<ReBook> ReBook { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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