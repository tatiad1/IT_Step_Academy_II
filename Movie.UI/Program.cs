using Microsoft.Extensions.DependencyInjection;
using Movie.Application.Implementations;
using Movie.Application.Interfaces;
using Movie.Domain.DTOs;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;
using Movie.Infrastructure.Repositories;

namespace Movie.UI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddScoped<MovieDbContext>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
            var movieService = scope.ServiceProvider.GetRequiredService<IMovieService>();

            // ---------- SEED ----------
            await DbSeeder.SeedAsync(dbContext);

            // ---------- LIST ----------
            Console.WriteLine("=== ALL MOVIES (initial) ===");
            await PrintMoviesAsync(movieService);

            var firstMovie = (await movieService.GetAllMoviesAsync()).First();
            var lastMovie = (await movieService.GetAllMoviesAsync()).Last();

            // ---------- UPDATE ----------
            Console.WriteLine($"\nUpdating movie with Id {firstMovie.Id}...");
            var updated = await movieService.UpdateMovieAsync(new UpdateMovieDTO
            {
                Id = firstMovie.Id,
                Title = "The Dark Knight (Updated)",
                ReleaseYear = 2009,
                StudioId = 1 // change to a StudioId that exists in your DB if needed
            });
            Console.WriteLine(updated ? "Movie updated." : "Movie not found.");

            Console.WriteLine("\n=== ALL MOVIES (after update) ===");
            await PrintMoviesAsync(movieService);

            // ---------- DELETE ----------
            Console.WriteLine($"\nDeleting movie with Id {lastMovie.Id}...");
            var deleted = await movieService.DeleteMovieAsync(lastMovie.Id);
            Console.WriteLine(deleted ? "Movie deleted." : "Movie not found.");

            Console.WriteLine("\n=== ALL MOVIES (after delete) ===");
            await PrintMoviesAsync(movieService);
        }

        private static async Task PrintMoviesAsync(IMovieService movieService)
        {
            var movies = await movieService.GetAllMoviesAsync();

            foreach (var m in movies)
                Console.WriteLine($"{m.Id} | {m.Title} | {m.ReleaseYear} | {m.StudioName}");
        }
    }
}