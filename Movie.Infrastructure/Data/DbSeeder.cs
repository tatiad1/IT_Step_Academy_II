using Movie.Domain.Entities;
using MovieEntity = Movie.Domain.Entities.Movie; // alias avoids clash between namespace "Movie" and class "Movie"

namespace Movie.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(MovieDbContext context)
        {
            // Only seed if the database is empty, so running twice doesn't duplicate data
            if (context.Movies.Any())
                return;

            // Countries
            var usa = new Country { Name = "USA" };
            var uk = new Country { Name = "United Kingdom" };

            // Studios (+ one-to-one StudioDetails)
            var warner = new Studio
            {
                Name = "Warner Bros",
                Country = usa,
                StudioDetails = new StudioDetails { LicenseNumber = 1001 }
            };
            var universal = new Studio
            {
                Name = "Universal Pictures",
                Country = usa,
                StudioDetails = new StudioDetails { LicenseNumber = 1002 }
            };
            var pinewood = new Studio
            {
                Name = "Pinewood Studios",
                Country = uk,
                StudioDetails = new StudioDetails { LicenseNumber = 2001 }
            };

            // Actors
            var bale = new Actor { FirstName = "Christian", LastName = "Bale" };
            var ledger = new Actor { FirstName = "Heath", LastName = "Ledger" };
            var dicaprio = new Actor { FirstName = "Leonardo", LastName = "DiCaprio" };
            var craig = new Actor { FirstName = "Daniel", LastName = "Craig" };

            // Movies (+ many-to-many Actors)
            var movies = new List<MovieEntity>
            {
                new MovieEntity
                {
                    Title = "The Dark Knight",
                    ReleaseYear = 2008,
                    Studio = warner,
                    Actors = new List<Actor> { bale, ledger }
                },
                new MovieEntity
                {
                    Title = "Inception",
                    ReleaseYear = 2010,
                    Studio = warner,
                    Actors = new List<Actor> { dicaprio }
                },
                new MovieEntity
                {
                    Title = "Jurassic World",
                    ReleaseYear = 2015,
                    Studio = universal,
                    Actors = new List<Actor>()
                },
                new MovieEntity
                {
                    Title = "Skyfall",
                    ReleaseYear = 2012,
                    Studio = pinewood,
                    Actors = new List<Actor> { craig }
                }
            };

            // EF Core inserts the related Countries, Studios, StudioDetails and Actors automatically
            await context.Movies.AddRangeAsync(movies);
            await context.SaveChangesAsync();
        }
    }
}