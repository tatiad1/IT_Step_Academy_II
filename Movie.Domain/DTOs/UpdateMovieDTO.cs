namespace Movie.Domain.DTOs
{
    public class UpdateMovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int StudioId { get; set; }
    }
}