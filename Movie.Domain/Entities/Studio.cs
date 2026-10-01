using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Movie.Domain.Entities
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public StudioDetails StudioDetails { get; set; }
        public ICollection<Movie> Movies { get; set; }

    }
}
