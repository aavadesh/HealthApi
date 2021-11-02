
using Microsoft.EntityFrameworkCore;

namespace HealthApi.Entities
{
    public class HealthDbContext : DbContext
    {
        public HealthDbContext(DbContextOptions<HealthDbContext> options) : base(options)
        {
            
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookContent> BookContents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookContent>().HasKey();
            modelBuilder.Entity<BookCategory>()
            .HasKey(bc => new { bc.BookId, bc.CategoryId });
            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookCategory>().HasKey(vf => new { vf.BookId, vf.CategoryId });
            modelBuilder.Entity<AuthorBook>()
          .HasKey(bc => new { bc.BookId, bc.AuthorId });
            modelBuilder.Entity<AuthorBook>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.AuthorBooks)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<AuthorBook>()
                .HasOne(bc => bc.Author)
                .WithMany(c => c.AuthorBooks)
                .HasForeignKey(bc => bc.AuthorId);
            modelBuilder.Entity<BookCategory>().HasKey(sc => new { sc.BookId, sc.CategoryId });
            modelBuilder.Entity<BookCategory>()
                .HasOne<Book>(sc => sc.Book)
                .WithMany(s => s.BookCategories)
                .HasForeignKey(sc => sc.BookId);
            modelBuilder.Entity<BookCategory>()
                .HasOne<Category>(sc => sc.Category)
                .WithMany(s => s.BookCategories)
                .HasForeignKey(sc => sc.CategoryId);
        }
    }
}
