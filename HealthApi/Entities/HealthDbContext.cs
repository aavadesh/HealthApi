
using HealthApi.Models;
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
        public virtual DbSet<SearchDto> Search { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCategory>().HasKey(sc => new { sc.CategoryId, sc.BookId });
            modelBuilder.Entity<AuthorBook>().HasKey(sc => new { sc.BookId, sc.AuthorId });
            // Necessary, since our model isnt a EF model
            modelBuilder.Entity<SearchDto>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<Role>()
        .HasData(
            new Role
            {
                Id = 1,
                Name = "user"
            },
            new Role
            {
                Id = 2,
                Name = "admin"
            }
        );
        }
    }
}
