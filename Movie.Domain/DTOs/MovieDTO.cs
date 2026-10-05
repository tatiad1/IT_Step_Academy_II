namespace Movie.Domain.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string StudioName { get; set; }

        public override string ToString()
        {
            return $"{Id} | {Title} | {ReleaseYear} | {StudioName}";
        }
    }
}
