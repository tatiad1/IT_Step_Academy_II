using Movie.Application.Interfaces;
using Movie.Domain.DTOs;
using Movie.Domain.Entities;
using Movie.Domain.Interfaces;

namespace Movie.Application.Implementations
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActorService(IActorRepository actorRepository, IUnitOfWork unitOfWork)
        {
            _actorRepository = actorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<ActorDTO>> GetAllActorsAsync()
        {
            var actors = await _actorRepository.GetAllActorsAsync();

            var actorsDTOs = actors.Select(a => new ActorDTO
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                MovieTitles = a.Movies.Select(m => m.Title).ToList()
            }).ToList();

            return actorsDTOs;
        }

        public async Task<ActorDTO> GetActorByIDAsync(int id)
        {
            var actor = await _actorRepository.GetActorByIDAsync(id);

            if (actor == null)
            {
                return null;
            }

            return new ActorDTO
            {
                Id = actor.Id,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                MovieTitles = actor.Movies.Select(m => m.Title).ToList()
            };
        }

        public async Task AddActorAsync(CreateActorDTO actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            var newActor = new Actor
            {
                FirstName = actor.FirstName,
                LastName = actor.LastName
            };

            await _actorRepository.AddActorAsync(newActor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateActorAsync(int id, UpdateActorDTO actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            var newActor = new Actor
            {
                FirstName = actor.FirstName,
                LastName = actor.LastName
            };

            await _actorRepository.UpdateActorAsync(id, newActor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteActorAsync(int id)
        {
            var result = await _actorRepository.DeleteActorAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            return result;
        }

        public async Task UpdateActorMovieAsync(int actorId, UpdateActorMovieDTO actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            await _actorRepository.UpdateActorMoviesAsync(actorId, actor.MovieIds);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}