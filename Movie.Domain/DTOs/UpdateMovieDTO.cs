using System.ComponentModel.DataAnnotations;

namespace Movie.Domain.DTOs
{
    public class UpdateMovieDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Range(1888, 2100, ErrorMessage = "Release year must be between 1888 and 2100.")]
        public int ReleaseYear { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Studio ID must be a positive number.")]
        public int StudioId { get; set; }
    }
}