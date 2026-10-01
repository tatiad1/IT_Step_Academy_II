using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int StudioId { get; set; }
        public Studio Studio { get; set; }
        public ICollection<Actor> Actors { get; set; }
    }
}
