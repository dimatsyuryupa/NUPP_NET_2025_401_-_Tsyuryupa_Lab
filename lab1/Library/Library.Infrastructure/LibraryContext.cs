using Microsoft.EntityFrameworkCore;
using Library.Common;

namespace Library.Infrastructure
{
    public class LibraryContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }

        public LibraryContext() { }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ⚙️ Підключення до локальної бази (SQLite)
                optionsBuilder.UseSqlite("Data Source=library.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === Базовий клас Person ===
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FullName).IsRequired().HasMaxLength(100);
            });

            // === Table-per-Type (TPT) для Author і Librarian ===
            modelBuilder.Entity<Author>().ToTable("Authors");
            modelBuilder.Entity<Librarian>().ToTable("Librarians");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(a => a.Nationality).HasMaxLength(50);
                entity.Property(a => a.BooksPublished).IsRequired();
            });

            modelBuilder.Entity<Librarian>(entity =>
            {
                entity.Property(l => l.Position).HasMaxLength(100);
            });

            // === Book ===
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Genre).HasMaxLength(50);

                entity.HasOne(b => b.Author)
                      .WithMany()
                      .HasForeignKey("AuthorId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // === Bus ===
            modelBuilder.Entity<Bus>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Model).HasMaxLength(50);
                entity.Property(b => b.Seats).IsRequired();
                entity.Property(b => b.Speed).IsRequired();
            });

            // === LibraryCard ===
            modelBuilder.Entity<LibraryCard>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Number).IsRequired().HasMaxLength(50);
                entity.Property(c => c.IssuedDate).IsRequired();

                entity.HasOne(c => c.Owner)
                      .WithMany()
                      .HasForeignKey("OwnerId")
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
