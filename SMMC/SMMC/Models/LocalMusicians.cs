using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class LocalMusicians
    {
        public LocalMusicians()
        {
            LocalMusiciansEnsemble = new HashSet<LocalMusiciansEnsemble>();
        }

        public int LocalMusiciansId { get; set; }
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Contact Contact { get; set; }
        public ICollection<LocalMusiciansEnsemble> LocalMusiciansEnsemble { get; set; }
    }
}
