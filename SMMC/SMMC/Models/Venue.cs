using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Venue
    {
        public Venue()
        {
            Performance = new HashSet<Performance>();
        }

        public int VenueId { get; set; }
        public string VenueName { get; set; }

        public ICollection<Performance> Performance { get; set; }
    }
}
