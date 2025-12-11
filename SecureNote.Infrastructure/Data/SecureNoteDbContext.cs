using Microsoft.EntityFrameworkCore;
using SecureNote.Domain.Entities;

namespace SecureNote.Infrastructure.Data
{
    // DbContext, Entity Framework'ün kalbidir.
    public class SecureNoteDbContext : DbContext
    {
        // Constructor: Dışarıdan (API'den) ayarları (Connection String) alır.
        public SecureNoteDbContext(DbContextOptions<SecureNoteDbContext> options) : base(options)
        {
        }

        // Veritabanındaki Tablolarımız
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        // İlişkileri ve kısıtlamaları burada ince ayar yaparız (Fluent API).
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            // User Tablosu Ayarları
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Aynı mail ile ikinci kez kayıt olunamaz! (Veritabanı seviyesinde koruma)

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // İlişki Tanımı: Bir User'ın çok Note'u vardır.
            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)     
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse notları da silinsin.

            base.OnModelCreating(modelBuilder);
        }
    }
}