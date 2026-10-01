using Microsoft.EntityFrameworkCore;
using Movie.Domain.Entities;
using MovieEntity = Movie.Domain.Entities.Movie; // alias avoids clash between namespace "Movie" and class "Movie"

namespace Movie.Infrastructure.Data
{
    public class MovieDbContext : DbContext
    {
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudioDetails> StudioDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;Database=MoviesDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Country
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);

                // One-to-Many: Country -> Studios
                entity.HasMany(c => c.Studios)
                      .WithOne(s => s.Country)
                      .HasForeignKey(s => s.CountryId);
            });

            // Studio
            modelBuilder.Entity<Studio>(entity =>
            {
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);

                // One-to-One: Studio -> StudioDetails (FK in StudioDetails)
                entity.HasOne(s => s.StudioDetails)
                      .WithOne(d => d.Studio)
                      .HasForeignKey<StudioDetails>(d => d.StudioId);

                // One-to-Many: Studio -> Movies
                entity.HasMany(s => s.Movies)
                      .WithOne(m => m.Studio)
                      .HasForeignKey(m => m.StudioId);
            });

            // StudioDetails
            modelBuilder.Entity<StudioDetails>(entity =>
            {
                entity.Property(d => d.LicenseNumber).IsRequired();
            });

            // Movie
            modelBuilder.Entity<MovieEntity>(entity =>
            {
                entity.Property(m => m.Title).IsRequired().HasMaxLength(150);

                // Many-to-Many: Movie <-> Actor with join table "MovieActors"
                entity.HasMany(m => m.Actors)
                      .WithMany(a => a.Movies)
                      .UsingEntity(j => j.ToTable("MovieActors"));
            });

            // Actor
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            });
        }
    }
}