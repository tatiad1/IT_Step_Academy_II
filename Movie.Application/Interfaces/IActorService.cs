using Movie.Domain.DTOs;

namespace Movie.Application.Interfaces
{
    public interface IActorService
    {
        Task<ICollection<ActorDTO>> GetAllActorsAsync();
        Task<ActorDTO> GetActorByIDAsync(int id);
        Task AddActorAsync(CreateActorDTO actor);
        Task UpdateActorAsync(int id, UpdateActorDTO actor);
        Task<bool> DeleteActorAsync(int id);
        Task UpdateActorMovieAsync(int actorId, UpdateActorMovieDTO actor);
    }
}