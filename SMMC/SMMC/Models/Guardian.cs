using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Guardian
    {
        public Guardian()
        {
            StudentGuardian = new HashSet<StudentGuardian>();
        }

        public int GuardianId { get; set; }
        public int ContactId { get; set; }
        public int PersonId { get; set; }

        public Contact Contact { get; set; }
        public Person GuardianNavigation { get; set; }
        public ICollection<StudentGuardian> StudentGuardian { get; set; }
    }
}
