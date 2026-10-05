using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.DTOs
{
    public class CreateMovieDTO
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int StudioId { get; set; } 
    }
}
