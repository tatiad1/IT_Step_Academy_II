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
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
            var movieService = scope.ServiceProvider.GetRequiredService<IMovieService>();
            var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();

            // ---------- SEED ----------
            await DbSeeder.SeedAsync(dbContext);

            // =====================================================
            //                      MOVIES
            // =====================================================
            Console.WriteLine("=== ALL MOVIES (initial) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await movieService.GetAllMoviesAsync()));

            // ---------- ADD ----------
            Console.WriteLine("\nAdding movie 'Test Movie'...");
            await movieService.AddMovieAsync(new CreateMovieDTO
            {
                Title = "Test Movie",
                ReleaseYear = 2020,
                StudioId = 1 // Warner Bros on a fresh database; use a real StudioId if yours differs
            });

            Console.WriteLine("\n=== ALL MOVIES (after add) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await movieService.GetAllMoviesAsync()));

            var testMovie = (await movieService.GetAllMoviesAsync()).First(m => m.Title == "Test Movie");

            // ---------- GET BY ID ----------
            Console.WriteLine($"\nGet movie by Id {testMovie.Id}:");
            Console.WriteLine(await movieService.GetMovieByIDAsync(testMovie.Id));

            // ---------- UPDATE ----------
            Console.WriteLine($"\nUpdating movie with Id {testMovie.Id}...");
            var updated = await movieService.UpdateMovieAsync(new UpdateMovieDTO
            {
                Id = testMovie.Id,
                Title = "Test Movie (Updated)",
                ReleaseYear = 2021,
                StudioId = 1
            });
            Console.WriteLine(updated ? "Movie updated." : "Movie not found.");

            Console.WriteLine("\n=== ALL MOVIES (after update) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await movieService.GetAllMoviesAsync()));

            // ---------- DELETE ----------
            Console.WriteLine($"\nDeleting movie with Id {testMovie.Id}...");
            var deleted = await movieService.DeleteMovieAsync(testMovie.Id);
            Console.WriteLine(deleted ? "Movie deleted." : "Movie not found.");

            Console.WriteLine("\n=== ALL MOVIES (after delete) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await movieService.GetAllMoviesAsync()));

            // =====================================================
            //                      ACTORS
            // =====================================================
            Console.WriteLine("\n\n=== ALL ACTORS (initial) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await actorService.GetAllActorsAsync()));

            // ---------- ADD ----------
            Console.WriteLine("\nAdding actor 'Tom Hardy'...");
            await actorService.AddActorAsync(new CreateActorDTO
            {
                FirstName = "Tom",
                LastName = "Hardy"
            });

            var testActor = (await actorService.GetAllActorsAsync()).First(a => a.LastName == "Hardy");

            Console.WriteLine("\n=== ALL ACTORS (after add) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await actorService.GetAllActorsAsync()));

            // ---------- GET BY ID ----------
            Console.WriteLine($"\nGet actor by Id {testActor.Id}:");
            Console.WriteLine(await actorService.GetActorByIDAsync(testActor.Id));

            // ---------- UPDATE NAME ----------
            Console.WriteLine($"\nUpdating actor with Id {testActor.Id}...");
            await actorService.UpdateActorAsync(testActor.Id, new UpdateActorDTO
            {
                FirstName = "Thomas",
                LastName = "Hardy"
            });

            // ---------- UPDATE MOVIES (many-to-many) ----------
            var movieIds = (await movieService.GetAllMoviesAsync()).Take(2).Select(m => m.Id).ToList();
            Console.WriteLine($"\nAssigning actor {testActor.Id} to movies: {string.Join(", ", movieIds)}...");
            await actorService.UpdateActorMovieAsync(testActor.Id, new UpdateActorMovieDTO
            {
                MovieIds = movieIds
            });

            Console.WriteLine("\n=== ALL ACTORS (after update + movies) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await actorService.GetAllActorsAsync()));

            // ---------- REPLACE MOVIES (old links should be removed) ----------
            var oneMovieId = movieIds.Take(1).ToList();
            Console.WriteLine($"\nReplacing movies of actor {testActor.Id} with: {string.Join(", ", oneMovieId)}...");
            await actorService.UpdateActorMovieAsync(testActor.Id, new UpdateActorMovieDTO
            {
                MovieIds = oneMovieId
            });

            Console.WriteLine("\n=== ALL ACTORS (after replacing movies) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await actorService.GetAllActorsAsync()));

            // ---------- INVALID MOVIE ID ----------
            Console.WriteLine("\nTrying to assign a movie ID that doesn't exist (9999)...");
            try
            {
                await actorService.UpdateActorMovieAsync(testActor.Id, new UpdateActorMovieDTO
                {
                    MovieIds = new List<int> { 9999 }
                });
                Console.WriteLine("No error (unexpected).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error caught as expected: {ex.Message}");
            }

            // ---------- DELETE ----------
            Console.WriteLine($"\nDeleting actor with Id {testActor.Id}...");
            var actorDeleted = await actorService.DeleteActorAsync(testActor.Id);
            Console.WriteLine(actorDeleted ? "Actor deleted." : "Actor not found.");

            Console.WriteLine("\n=== ALL ACTORS (after delete) ===");
            Console.WriteLine(string.Join(Environment.NewLine, await actorService.GetAllActorsAsync()));

            // =====================================================
            //                    EDGE CASES
            // =====================================================
            Console.WriteLine("\n\n=== EDGE CASES ===");

            var missingMovie = await movieService.GetMovieByIDAsync(9999);
            Console.WriteLine(missingMovie == null
                ? "GetMovieByID(9999): null returned (OK)"
                : "GetMovieByID(9999): unexpected result");

            var missingActor = await actorService.GetActorByIDAsync(9999);
            Console.WriteLine(missingActor == null
                ? "GetActorByID(9999): null returned (OK)"
                : "GetActorByID(9999): unexpected result");

            var missingUpdate = await movieService.UpdateMovieAsync(new UpdateMovieDTO
            {
                Id = 9999,
                Title = "Nothing",
                ReleaseYear = 2000,
                StudioId = 1
            });
            Console.WriteLine($"UpdateMovie(9999) returned: {missingUpdate} (expected False)");

            var missingDelete = await movieService.DeleteMovieAsync(9999);
            Console.WriteLine($"DeleteMovie(9999) returned: {missingDelete} (expected False)");

            try
            {
                await actorService.DeleteActorAsync(9999);
                Console.WriteLine("DeleteActor(9999): no error (unexpected)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteActor(9999) threw: {ex.Message}");
            }

            // =====================================================
            //                 SEARCH (Tasks 1-3)
            // =====================================================
            void Show(ICollection<MovieDTO> result)
            {
                Console.WriteLine(result.Count == 0
                    ? "(no results)"
                    : string.Join(Environment.NewLine, result));
            }

            Console.WriteLine("\n\n=== TASK 1: SearchMoviesByStudio ===");

            Console.WriteLine("\nyear >= 2000, 'Warner Bros', at least 1 actor:");
            Show(await movieService.SearchMoviesByStudioAsync(2000, "Warner Bros", 1));

            Console.WriteLine("\nyear >= 2000, 'Warner Bros', at least 2 actors:");
            Show(await movieService.SearchMoviesByStudioAsync(2000, "Warner Bros", 2));

            Console.WriteLine("\nyear >= 2009, 'Warner Bros', at least 1 actor:");
            Show(await movieService.SearchMoviesByStudioAsync(2009, "Warner Bros", 1));

            Console.WriteLine("\nyear >= 2000, 'Universal Pictures', at least 0 actors:");
            Show(await movieService.SearchMoviesByStudioAsync(2000, "Universal Pictures", 0));

            Console.WriteLine("\nyear >= 2000, 'Nonexistent Studio', at least 0 actors:");
            Show(await movieService.SearchMoviesByStudioAsync(2000, "Nonexistent Studio", 0));

            Console.WriteLine("\n\n=== TASK 2: SearchMoviesByCountry ===");

            Console.WriteLine("\n'USA', year >= 2000, at most 5 actors:");
            Show(await movieService.SearchMoviesByCountryAsync("USA", 2000, 5));

            Console.WriteLine("\n'USA', year >= 2000, at most 1 actor:");
            Show(await movieService.SearchMoviesByCountryAsync("USA", 2000, 1));

            Console.WriteLine("\n'United Kingdom', year >= 2000, at most 5 actors:");
            Show(await movieService.SearchMoviesByCountryAsync("United Kingdom", 2000, 5));

            Console.WriteLine("\n\n=== TASK 3: SearchMoviesAdvanced ===");

            Console.WriteLine("\n2000-2020, 'USA', any title, at least 0 actors:");
            Show(await movieService.SearchMoviesAdvancedAsync(2000, 2020, "USA", "", 0));

            Console.WriteLine("\n2000-2020, 'USA', title contains 'dark', at least 1 actor:");
            Show(await movieService.SearchMoviesAdvancedAsync(2000, 2020, "USA", "dark", 1));

            Console.WriteLine("\n2009-2020, 'USA', any title, at least 0 actors:");
            Show(await movieService.SearchMoviesAdvancedAsync(2009, 2020, "USA", "", 0));

            Console.WriteLine("\n2000-2020, 'United Kingdom', title contains 'sky', at least 1 actor:");
            Show(await movieService.SearchMoviesAdvancedAsync(2000, 2020, "United Kingdom", "sky", 1));

            Console.WriteLine("\n2000-2020, 'USA', title contains 'xyz', at least 0 actors:");
            Show(await movieService.SearchMoviesAdvancedAsync(2000, 2020, "USA", "xyz", 0));
        }
    }
}