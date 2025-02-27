using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Database_Tool
{
    public class Library : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=Ujifyxbr2511");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // <<-- Конфиг авторов -->>

            modelBuilder.Entity<Author>()
                .HasKey(a => a.ID);

            modelBuilder.Entity<Author>()
                .Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Author>()
                .Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Author>()
                .Property(a => a.Country)
                .HasMaxLength(100);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.AuthorClass)
                .HasForeignKey(b => b.Author);

            // <<-- Конфиг книг -->>

            modelBuilder.Entity<Book>()
                .HasKey(b => b.ID);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.AuthorClass)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.Author)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.GenreClass)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.Genre)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .Property(b=>b.Title)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(b => b.ISBN)
                .IsRequired();

            // <<-- Конфиг жанров -->>
            modelBuilder.Entity<Genre>()
                .HasKey(g => g.ID);

            modelBuilder.Entity<Genre>()
                .Property(g => g.Name)
                .IsRequired();

            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Books)
                .WithOne(b => b.GenreClass)
                .HasForeignKey(b => b.Genre);
        }
    }
}
