using Microsoft.EntityFrameworkCore;
using Movie.Domain.Entities;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;

namespace Movie.Infrastructure.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public ActorRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task AddActorAsync(Actor actor)
        {
            await _movieDbContext.Actors.AddAsync(actor);
        }

        public async Task<ICollection<Actor>> GetAllActorsAsync()
        {
            return await _movieDbContext.Actors
                .Include(a => a.Movies)
                .ToListAsync();
        }

        public async Task<Actor> GetActorByIDAsync(int id)
        {
            var actorByID = await _movieDbContext.Actors
                .Include(a => a.Movies)
                .FirstOrDefaultAsync(a => a.Id == id);

            return actorByID;
        }

        public async Task UpdateActorAsync(int actorId, Actor actor)
        {
            var actorExists = await _movieDbContext.Actors.FirstOrDefaultAsync(a => a.Id == actorId);
            if (actorExists == null)
            {
                throw new Exception($"Actor with ID {actorId} not found.");
            }

            actorExists.FirstName = actor.FirstName;
            actorExists.LastName = actor.LastName;
        }

        public async Task UpdateActorMoviesAsync(int actorId, ICollection<int> movieIds)
        {
            if (movieIds == null)
            {
                throw new ArgumentNullException(nameof(movieIds));
            }

            var actor = await _movieDbContext.Actors
                .Include(a => a.Movies)
                .FirstOrDefaultAsync(a => a.Id == actorId);

            if (actor == null)
            {
                throw new Exception($"Actor with ID {actorId} not found.");
            }

            var distinctIds = movieIds.Distinct().ToList();

            var movies = await _movieDbContext.Movies
                .Where(m => distinctIds.Contains(m.Id))
                .ToListAsync();

            if (movies.Count != distinctIds.Count)
            {
                throw new Exception("One or more movie IDs were not found.");
            }

            actor.Movies.Clear();
            foreach (var movie in movies)
            {
                actor.Movies.Add(movie);
            }
        }

        public async Task<bool> DeleteActorAsync(int id)
        {
            var actorToDelete = await _movieDbContext.Actors.FindAsync(id);

            if (actorToDelete == null)
            {
                throw new Exception($"Actor with ID {id} not found.");
            }

            _movieDbContext.Actors.Remove(actorToDelete);
            return true;
        }
    }
}