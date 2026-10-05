using Movie.Domain.Entities;

namespace Movie.Domain.Interfaces
{
    public interface IActorRepository
    {
        Task AddActorAsync(Actor actor);

        Task<ICollection<Actor>> GetAllActorsAsync();

        Task<Actor> GetActorByIDAsync(int id);

        Task UpdateActorAsync(int actorId, Actor actor);
        Task UpdateActorMoviesAsync(int actorId, ICollection<int> movieIds);

        Task<bool> DeleteActorAsync(int id);
    }
}