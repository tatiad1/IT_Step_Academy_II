namespace Movie.Domain.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<string> MovieTitles { get; set; }

        public override string ToString()
        {
            return $"{Id} | {FirstName} {LastName} | Movies: {string.Join(", ", MovieTitles ?? new List<string>())}";
        }
    }
}