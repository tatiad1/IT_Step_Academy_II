namespace Movie.Domain.DTOs
{
    public class UpdateActorMovieDTO
    {
        public ICollection<int> MovieIds { get; set; }
    }
}
