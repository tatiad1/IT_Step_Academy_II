using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.Entities
{
    public class StudioDetails
    {
        public int Id { get; set; }
        public int LicenseNumber { get; set; }
        public int StudioId { get; set; }
        public Studio Studio { get; set; }
    }
}
